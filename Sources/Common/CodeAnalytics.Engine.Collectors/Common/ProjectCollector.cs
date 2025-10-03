using System.Diagnostics;
using CodeAnalytics.Engine.Collectors.Models.Contexts;
using CodeAnalytics.Engine.Collectors.Options;
using CodeAnalytics.Engine.Collectors.Symbols.Syntax;
using CodeAnalytics.Engine.Storage.Common;
using CodeAnalytics.Engine.Storage.Extensions;
using CodeAnalytics.Engine.Storage.Models.Structure;
using Me.Memory.Results;
using Me.Memory.Results.Errors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CodeAnalytics.Engine.Collectors.Common;

public sealed partial class ProjectCollector : IAsyncDisposable
{
   private readonly ILogger<ProjectCollector> _logger;

   private readonly IOptionsMonitor<CollectorOptions> _optionsMonitor;
   private CollectorOptions CollectorOptions => _optionsMonitor.CurrentValue;
   
   private CollectContext? _context;

   public ProjectCollector(
      ILogger<ProjectCollector> logger, 
      IOptionsMonitor<CollectorOptions> optionsMonitor)
   {
      _logger = logger;
      _optionsMonitor = optionsMonitor;
   }

   public async Task<Result<int, Error<string>>> Collect(
      DbMainContext dbContext,
      DbSolution dbSolution,
      Workspace workSpace,
      Project project,
      CancellationToken ct)
   {
      var start = Stopwatch.GetTimestamp();
      var compilation = await project.GetCompilationAsync(ct);
      
      if (compilation is null)
      {
         return new Error<string>("Compilation couldn't be resolved from project.");
      }
      
      var loadingTime = new TimeSpan(Stopwatch.GetTimestamp() - start);
      start = Stopwatch.GetTimestamp();
      
      LogStartProjectCollect(project.FilePath ?? string.Empty);
      LogStartupTime(loadingTime);
      
      var nodesIterated = 0;
      var dbProject = await GetOrCreateDbProject(dbContext, dbSolution, project, ct);

      foreach (var tree in compilation.SyntaxTrees)
      {
         var semanticModel = compilation.GetSemanticModel(tree, ignoreAccessibility: true);
         var root = await tree.GetRootAsync(ct);

         var document = workSpace.CurrentSolution.GetDocument(tree);
         if (document is null) continue;
         
         var filePath = Path.GetRelativePath(CollectorOptions.BasePath, tree.FilePath);
         var dbFile = await GetOrCreateDbFile(dbContext, dbProject, filePath, ct);

         _context = new CollectContext()
         {
            Compilation = compilation,
            Options = CollectorOptions,
            SourceText = await tree.GetTextAsync(ct),
            SemanticModel = semanticModel,
            SyntaxNode = root,
            SyntaxTree = tree,
            Document = document,
            CancellationToken = ct,
            DbContext = dbContext,
            DbSolution = dbSolution,
            DbProject = dbProject,
            DbFile = dbFile,
         };
         
         // write text --
         
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

            await HandleNode(_context);
         }
      }
      
      loadingTime = new TimeSpan(Stopwatch.GetTimestamp() - start);
      LogNodesRan(nodesIterated, loadingTime);

      return nodesIterated;
   }

   private async Task HandleNode(CollectContext context)
   {
      switch (context.SyntaxNode)
      {
         case ClassDeclarationSyntax classDeclarationSyntax:
            if (context.Symbol is not null) LogFound(context.Symbol.Kind);
            await ClassDeclarationTransformer.TryTransform(classDeclarationSyntax, context);
            break;
         case StructDeclarationSyntax structDeclarationSyntax:
            if (context.Symbol is not null) LogFound(context.Symbol.Kind);
            await StructDeclarationTransformer.TryTransform(structDeclarationSyntax, context);
            break;
         case InterfaceDeclarationSyntax interfaceDeclarationSyntax:
            if (context.Symbol is not null) LogFound(context.Symbol.Kind);
            await InterfaceDeclarationTransformer.TryTransform(interfaceDeclarationSyntax, context);
            break;
         case EnumDeclarationSyntax enumDeclarationSyntax:
            if (context.Symbol is not null) LogFound(context.Symbol.Kind);
            await EnumDeclarationTransformer.TryTransform(enumDeclarationSyntax, context);
            break;
      }
   }

   private async Task<DbFile> GetOrCreateDbFile(
      DbMainContext context,
      DbProject dbProject,
      string filePath,
      CancellationToken ct)
   {
      using var attachContext = context.AttachContext(dbProject);

      var dbFile = new DbFile()
      {
         Name = Path.GetFileName(filePath),
         RelativeFilePath = filePath,
         Projects = [dbProject]
      };

      return await context.UpdateOrCreate(context.Files)
         .Match(x => x.RelativeFilePath == filePath)
         .Include(x => x.Include(e => e.Projects))
         .OnCreate(() => dbFile)
         .OnUpdate(async (ctx, existing, token) =>
         {
            if (existing.Projects.Any(x => x.Id == dbProject.Id)) 
               return existing;
            
            using var attachInnerContext = ctx.AttachContext(existing);
            existing.Projects.Add(dbProject);

            await ctx.SaveChangesAsync(token);
            return existing;
         })
         .Execute(ct);
   }
   
   private async Task<DbProject> GetOrCreateDbProject(
      DbMainContext context,
      DbSolution dbSolution,
      Project project,
      CancellationToken ct)
   {
      using var attachContext = context.AttachContext(dbSolution);
      
      var projectPath = Path.GetRelativePath(CollectorOptions.BasePath, project.FilePath ?? string.Empty);
      var dbProject = new DbProject()
      {
         RelativeFilePath = projectPath,
         AssemblyName = project.AssemblyName,
         Name = Path.GetFileName(projectPath),
         Files = [],
         ReferencedProjects = [],
         Solutions = [dbSolution]
      };

      return await context.UpdateOrCreate(context.Projects)
         .Match(x => x.RelativeFilePath == projectPath)
         .Include(x => x.Include(e => e.Solutions))
         .OnCreate(() => dbProject)
         .OnUpdate(async (ctx, existing, token) =>
         {
            if (existing.Solutions.Any(x => x.Id == dbSolution.Id)) 
               return existing;
            
            using var attachInnerContext = ctx.AttachContext(existing);
            existing.Solutions.Add(dbSolution);
            
            await ctx.SaveChangesAsync(token);
            return existing;
         })
         .Execute(ct);
   }

   public ValueTask DisposeAsync()
   {
      return ValueTask.CompletedTask;
   }
}