using System.Diagnostics;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Collectors.Interfaces;
using CodeAnalytics.Engine.Collector.Collectors.Options;
using CodeAnalytics.Engine.Collector.Syntax.Interfaces;
using CodeAnalytics.Engine.Collector.Syntax.Providers;
using CodeAnalytics.Engine.Extensions.Database;
using CodeAnalytics.Engine.Storage.Contexts;
using CodeAnalytics.Engine.Storage.Models.Structure;
using Me.Memory.Results;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Me.Memory.Results.Errors;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProjectReference = CodeAnalytics.Engine.Storage.Models.Structure.ProjectReference;

namespace CodeAnalytics.Engine.Collector.Collectors;

public sealed partial class ProjectCollector : IProjectCollector
{
   private readonly ILogger<ProjectCollector> _logger;
   private readonly ProjectOptions _options;

   private CollectContext? _context;

   public ProjectCollector(ProjectOptions options)
   {
      _options = options;
      _logger = options.ServiceProvider.GetRequiredService<ILogger<ProjectCollector>>();
   }

   public async Task<Result<int, Error<string>>> Collect(
      SolutionReference solutionReference,
      Workspace workSpace, 
      Project project, 
      CancellationToken ct = default)
   {
      var start = Stopwatch.GetTimestamp();
      var compilation = await project.GetCompilationAsync(ct);
      
      await using var dbContext = new DbMainContext(_options.DatabaseFilePath);

      if (compilation is null)
      {
         return new Error<string>("Compilation couldn't be resolved from project.");
      }

      var loadingTime = new TimeSpan(Stopwatch.GetTimestamp() - start);
      start = Stopwatch.GetTimestamp();

      LogStartProjectCollect(_options.Path);
      LogStartupTime(loadingTime);
      
      var nodesIterated = 0;
      var projectReference = await GetProjectReference(dbContext, solutionReference, project, ct);
      
      foreach (var tree in compilation.SyntaxTrees)
      {
         var semanticModel = compilation.GetSemanticModel(tree, ignoreAccessibility: true);
         var root = await tree.GetRootAsync(ct);

         var document = workSpace.CurrentSolution.GetDocument(tree);
         if (document is null) continue;
         
         var filePath = Path.GetRelativePath(_options.BasePath, tree.FilePath);
         var fileReference = await GetFileReference(dbContext, projectReference, filePath, ct);

         _context = new CollectContext()
         {
            Compilation = compilation,
            Options = _options,
            SourceText = await tree.GetTextAsync(ct),
            SemanticModel = semanticModel,
            SyntaxNode = root,
            SyntaxTree = tree,
            Document = document,
            CancellationToken = ct,
            DbMainContext = dbContext,
            SolutionReference = solutionReference,
            ProjectReference = projectReference,
            FileReference = fileReference,
         };
         
         // if (_options.WriteSourceFiles)
         // {
         //    await HandleText(_context);
         // }
         
         foreach (var node in root.DescendantNodesAndSelf(
            descendIntoChildren: static (n) =>
               n is CompilationUnitSyntax
                  or NamespaceDeclarationSyntax
                  or FileScopedNamespaceDeclarationSyntax
                  or TypeDeclarationSyntax,
            descendIntoTrivia: false))
         {
            _context.SyntaxNode = node;
            _context.ResetSymbol();
            nodesIterated++;

            HandleNode(_context);
         }
      }

      loadingTime = new TimeSpan(Stopwatch.GetTimestamp() - start);
      LogNodesRan(nodesIterated, loadingTime);
   
      return nodesIterated;
   }
   
   private async Task<ProjectReference> GetProjectReference(
      DbMainContext context, 
      SolutionReference solutionReference,
      Project project, 
      CancellationToken ct = default)
   {
      var projectPath = Path.GetRelativePath(_options.BasePath, _options.Path);

      return await context.GetOrInsert(context.ProjectReferences, () => new ProjectReference()
         {
            RelativePath = projectPath,
            Name = project.Name,
            SolutionReferenceId = solutionReference.Id,
            ProjectReferences = [],
            FileReferences = [],
            SymbolDeclarations = [],
         },
         pr => pr.RelativePath == projectPath, 
         ct);
   }

   private async Task<FileReference> GetFileReference(
      DbMainContext context,
      ProjectReference projectReference,
      string filePath,
      CancellationToken ct)
   {
      return await context.GetOrInsert(context.FileReferences, () => new FileReference()
         {
            Name = Path.GetFileName(filePath),
            RelativePath = filePath,
            ProjectReferenceId = projectReference.Id,
            SymbolDeclarations = []
         },
         fr => fr.RelativePath == filePath,
         ct);
   }
   
   private void HandleNode(CollectContext context)
   {
      foreach (var provider in _syntaxProviders)
      {
         if (!provider.Predicator.Predicate(context))
         {
            continue;
         }
         
         provider.Transformer.Transform(context);
      }
   }
   
   public ValueTask DisposeAsync()
   {
      return ValueTask.CompletedTask;
   }
   
   private readonly List<ISyntaxProvider> _syntaxProviders = [
      new EnumSyntaxProvider(),
      new ClassSyntaxProvider(),
      new InterfaceSyntaxProvider(),
      new StructSyntaxProvider(),
   ];
}