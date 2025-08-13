using System.Diagnostics;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Collectors.Interfaces;
using CodeAnalytics.Engine.Collector.Collectors.Options;
using CodeAnalytics.Engine.Collector.Syntax.Providers;
using CodeAnalytics.Engine.Storage.Contexts;
using CodeAnalytics.Engine.Storage.Entities.Structure;
using CodeAnalytics.Engine.Storage.Extensions;
using Me.Memory.Results;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Me.Memory.Results.Errors;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
      DbSolution dbSolution,
      Workspace workSpace, 
      Project project, 
      CancellationToken ct = default)
   {
      var start = Stopwatch.GetTimestamp();
      var compilation = await project.GetCompilationAsync(ct);
      
      await using var dbContext = new DbMainContext(_options.DbConnectionString);

      if (compilation is null)
      {
         return new Error<string>("Compilation couldn't be resolved from project.");
      }

      var loadingTime = new TimeSpan(Stopwatch.GetTimestamp() - start);
      start = Stopwatch.GetTimestamp();

      LogStartProjectCollect(_options.Path);
      LogStartupTime(loadingTime);
      
      var nodesIterated = 0;
      var projectReference = await GetDbProject(dbContext, dbSolution, project, ct);
      
      foreach (var tree in compilation.SyntaxTrees)
      {
         var semanticModel = compilation.GetSemanticModel(tree, ignoreAccessibility: true);
         var root = await tree.GetRootAsync(ct);

         var document = workSpace.CurrentSolution.GetDocument(tree);
         if (document is null) continue;
         
         var filePath = Path.GetRelativePath(_options.BasePath, tree.FilePath);
         var fileReference = await GetDbFile(dbContext, projectReference, filePath, ct);

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
            DbSolution = dbSolution,
            DbProject = projectReference,
            DbFile = fileReference,
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
   
   private async Task<DbProject> GetDbProject(
      DbMainContext context, 
      DbSolution dbSolution,
      Project project, 
      CancellationToken ct = default)
   {
      var projectPath = Path.GetRelativePath(_options.BasePath, _options.Path);
      var dbProject = new DbProject()
      {
         RelativeFilePath = projectPath,
         AssemblyName = project.AssemblyName,
         Name = Path.GetFileName(projectPath),
         Files = [],
         ProjectReferences = [],
         Solutions = []
      };
      
      return await context.GetOrCreate(context.Projects)
         .Where(x => x.RelativeFilePath == projectPath)
         .OnCreate(() => dbProject)
         .Execute(ct);
   }

   private async Task<DbFile> GetDbFile(
      DbMainContext context,
      DbProject dbProject,
      string filePath,
      CancellationToken ct)
   {
      var dbFile = new DbFile()
      {
         Name = Path.GetFileName(filePath),
         RelativeFilePath = filePath,
         Projects = [dbProject]
      };
      
      return await context.GetOrCreate(context.Files)
         .Where(x => x.RelativeFilePath == filePath)
         .OnCreate(() => dbFile)
         .OnUpdate(x =>
         {
            x.Projects.Add(dbProject);
            return x;
         })
         .Execute(ct);
   }
   
   private void HandleNode(CollectContext context)
   {
      switch (context.SyntaxNode)
      {
         case var _ when _classSyntaxProvider.Predicator.Predicate(context):
            _classSyntaxProvider.Transformer.Transform(context);
            break;
         case var _ when _interfaceSyntaxProvider.Predicator.Predicate(context):
            _interfaceSyntaxProvider.Transformer.Transform(context);
            break;
         case var _ when _enumSyntaxProvider.Predicator.Predicate(context):
            _enumSyntaxProvider.Transformer.Transform(context);
            break;
         case var _ when _structSyntaxProvider.Predicator.Predicate(context):
            _structSyntaxProvider.Transformer.Transform(context);
            break;
      }
   }
   
   public ValueTask DisposeAsync()
   {
      return ValueTask.CompletedTask;
   }

   private readonly EnumSyntaxProvider _enumSyntaxProvider = new ();
   private readonly ClassSyntaxProvider _classSyntaxProvider = new ();
   private readonly InterfaceSyntaxProvider _interfaceSyntaxProvider = new ();
   private readonly StructSyntaxProvider _structSyntaxProvider = new ();
}