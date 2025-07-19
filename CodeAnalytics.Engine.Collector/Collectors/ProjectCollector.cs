using System.Diagnostics;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Collectors.Options;
using CodeAnalytics.Engine.Collector.Syntax.Interfaces;
using CodeAnalytics.Engine.Collector.Syntax.Providers;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Engine.Common.Results.Errors;
using CodeAnalytics.Engine.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodeAnalytics.Engine.Collector.Collectors;

public sealed partial class ProjectCollector
{
   private readonly ILogger<ProjectCollector> _logger;
   private readonly ProjectOptions _options;

   private CollectContext? _context;
   
   public ProjectCollector(ProjectOptions options)
   {
      _logger = options.ServiceProvider.GetRequiredService<ILogger<ProjectCollector>>();
      _options = options;
   }

   public async Task<Result<CollectorStore, Error<string>>> Collect(CancellationToken ct = default)
   {
      var stopwatch = Stopwatch.StartNew();
      var info = await Bootstrapper.GetProjectByPath(_options.ProjectPath, ct);
      
      if (info?.Compilation is null || info.Workspace is null)
      {
         return new Error<string>("Compilation or Workspace is null.");
      }

      using var workspace = info.Workspace;
      var store = new CollectorStore()
      {
         NodeIdStore = _options.NodeIdStore,
         StringIdStore = _options.StringIdStore,
         ComponentStore = new MergableComponentStore(_options.InitialCapacityPerComponentPool),
         LineCountStore = new LineCountStore()
      };

      LogStartProjectCollect(_options.ProjectPath);
      long nodesIterated = 0;

      foreach (var tree in info.Compilation.SyntaxTrees)
      {
         var semanticModel = info.Compilation.GetSemanticModel(tree, ignoreAccessibility: true);
         var root = await tree.GetRootAsync(ct);
         
         var document = workspace.CurrentSolution.GetDocument(tree);
         if (document is null) continue;
         
         _context = new CollectContext()
         {
            Compilation = info.Compilation,
            Options = _options,
            Store = store,
            SourceText = await tree.GetTextAsync(ct),
            SemanticModel = semanticModel,
            SyntaxNode = root,
            SyntaxTree = tree,
            Document = document,
            CancellationToken = ct,
         };

         foreach (var node in root.DescendantNodesAndSelf())
         {
            _context.SyntaxNode = node;
            nodesIterated++;
            
            HandleNode(_context);
         }
      }
      
      LogNodesRan(nodesIterated);
      return store;
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
   
   private readonly List<ISyntaxProvider> _syntaxProviders = [
      new EnumSyntaxProvider(),
      new ClassSyntaxProvider(),
      new InterfaceSyntaxProvider(),
      new StructSyntaxProvider(),
   ];
}