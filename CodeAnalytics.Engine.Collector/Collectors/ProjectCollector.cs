using System.Diagnostics;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Collectors.Options;
using CodeAnalytics.Engine.Collector.Syntax.Interfaces;
using CodeAnalytics.Engine.Collector.Syntax.Providers;
using CodeAnalytics.Engine.Collector.TextRendering;
using CodeAnalytics.Engine.Collector.TextRendering.Themes;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Engine.Common.Results.Errors;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.TextRendering;
using CodeAnalytics.Engine.Serialization.Collections;
using CodeAnalytics.Engine.Serialization.TextRendering;
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
      var start = Stopwatch.GetTimestamp();
      var info = await Bootstrapper.GetProjectByPath(_options.Path, ct);
      
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

      var loadingTime = new TimeSpan(Stopwatch.GetTimestamp() - start);
      start = Stopwatch.GetTimestamp();
      
      LogStartProjectCollect(_options.Path);
      LogStartupTime(loadingTime);
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
         await HandleText(_context);

         foreach (var node in root.DescendantNodesAndSelf())
         {
            _context.SyntaxNode = node;
            nodesIterated++;
            
            HandleNode(_context);
         }
      }

      loadingTime = new TimeSpan(Stopwatch.GetTimestamp() - start);
      
      LogNodesRan(nodesIterated, loadingTime);
      return store;
   }

   private async Task HandleText(CollectContext context)
   {
      var tokenizer = new TextTokenizer(context, CodeTheme.Default);
      var spans = await tokenizer.Tokenize();
      var writer = new ByteWriter(stackalloc byte[512]);
      byte[] data;
      string createPath;
      
      try
      {
         var fileRelativePath = context.GetRelativePath(context.SyntaxTree.FilePath);
         createPath = Path.Combine(context.Options.OutputBasePath, fileRelativePath);

         Directory.CreateDirectory(Path.GetDirectoryName(createPath) ?? string.Empty);
         createPath = Path.ChangeExtension(createPath, "csspan");

         PooledListSerializer<SyntaxSpan, SyntaxSpanSerializer>.Serialize(ref writer, ref spans);
         data = writer.WrittenSpan.ToArray();
      }
      finally
      {
         spans.Dispose();
         writer.Dispose();
      }

      await File.WriteAllBytesAsync(createPath, data);
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