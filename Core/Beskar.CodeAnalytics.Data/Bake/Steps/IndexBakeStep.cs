using Beskar.CodeAnalytics.Data.Bake.Interfaces;
using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Indexes;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Beskar.CodeAnalytics.Data.Indexes;
using Beskar.CodeAnalytics.Data.Indexes.Models;
using Microsoft.Extensions.Logging;

namespace Beskar.CodeAnalytics.Data.Bake.Steps;

public sealed class IndexBakeStep : IBakeStep
{
   public string Name => "Indexing";

   private readonly ILogger<IndexBakeStep> _logger;
   
   public IndexBakeStep(ILoggerFactory factory)
   {
      _logger = factory.CreateLogger<IndexBakeStep>();
   }
   
   public ValueTask Execute(BakeContext context, CancellationToken cancellationToken = default)
   {
      // Symbols
      new IndexBaker<SymbolSpec, StringFileView>(x => x.FullPathName, IndexType.NGram, "fullpathname")
         .Bake(context);
      
      // Method Symbols
      new IndexBaker<MethodSymbolSpec, int>(x => (byte)x.Parameters.Count, IndexType.StaticWideBTree, "parameter_count",
         Comparer<KeyedIndexEntry<int>>.Create((x, y) => x.Key - y.Key)).Bake(context);
      
      return ValueTask.CompletedTask;
   }
}