using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Bake.Interfaces;
using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Indexes;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Beskar.CodeAnalytics.Data.Indexes;
using Beskar.CodeAnalytics.Data.Indexes.Models;
using Beskar.CodeAnalytics.Data.Metadata.Indexes;
using Beskar.CodeAnalytics.Data.Metadata.Indexes.Cache;
using Beskar.CodeAnalytics.Data.Metadata.Specs;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;
using Microsoft.Extensions.Logging;

namespace Beskar.CodeAnalytics.Data.Bake.Steps;

public sealed class IndexBakeStep(ILoggerFactory factory) : IBakeStep
{
   public string Name => "Indexing";

   private readonly ILogger<IndexBakeStep> _logger = factory.CreateLogger<IndexBakeStep>();

   public ValueTask Execute(BakeContext context, CancellationToken cancellationToken = default)
   {
      // Symbols
      context.DatabaseBuilder.Symbols.SymbolIndexes = new SymbolSpecDescriptor.Indexes()
      {
         FullPathName = CreateNGramIndex<SymbolSpec>(context, IndexNames.Symbol.FullPathName, x => x.FullPathName)
      };
      
      // Method Symbols
      context.DatabaseBuilder.Symbols.MethodIndexes = new MethodSymbolSpecDescriptor.Indexes()
      {
         ParameterCount = CreateBTreeIndex<MethodSymbolSpec, int>(context, IndexNames.MethodSymbol.ParameterCount, x => x.Parameters.Count, Comparer<KeyedIndexEntry<int>>.Create((x, y) => x.Key - y.Key))
      };
      
      // Folder
      context.DatabaseBuilder.Structure.FolderIndexes = new FolderSpecDescriptor.Indexes
      {
         ParentId = CreateBTreeIndex<FolderSpec, uint>(context, IndexNames.Folder.ParentId, x => x.ParentId, Comparer<KeyedIndexEntry<uint>>.Create((x, y) => x.Key.CompareTo(y.Key)))
      };
      
      return ValueTask.CompletedTask;
   }

   private BTreeIndexDescriptor<TKey> CreateBTreeIndex<TEntity, TKey>(
      BakeContext context,
      string name, 
      Func<TEntity, TKey> selector,
      IComparer<KeyedIndexEntry<TKey>> comparer)
      where TEntity : unmanaged, ISpec
      where TKey : unmanaged, IComparable<TKey>
   {
      var fileName = new IndexBaker<TEntity, TKey>(selector, IndexType.StaticWideBTree, name, comparer).Bake(context);
      return new BTreeIndexDescriptor<TKey>()
      {
         Comparer = IndexComparerRegistry<TKey>.GetComparer(name),
         FileName = fileName,
         Name = name
      };
   }

   private NGramIndexDescriptor CreateNGramIndex<TEntity>(
      BakeContext context,
      string name, 
      Func<TEntity, StringFileView> selector)
      where TEntity : unmanaged, ISpec
   {
      var fileName = new IndexBaker<TEntity, StringFileView>(selector, IndexType.NGram, name).Bake(context);
      return new NGramIndexDescriptor()
      {
         FileName = fileName,
         Name = name
      };
   }
   
   [ModuleInitializer]
   [SuppressMessage("Usage", "CA2255:The \'ModuleInitializer\' attribute should not be used in libraries")]
   public static void RegisterComparers()
   {
      // Method Symbols
      IndexComparerRegistry<int>.Register(IndexNames.MethodSymbol.ParameterCount, Comparer<int>.Create((x, y) => x.CompareTo(y)));
      
      // Folder
      IndexComparerRegistry<uint>.Register(IndexNames.Folder.ParentId, Comparer<uint>.Create((x, y) => x.CompareTo(y)));
   }
}