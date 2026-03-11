using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Bake.Interfaces;
using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Indexes;
using Beskar.CodeAnalytics.Data.Enums.Storage;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Beskar.CodeAnalytics.Data.Indexes;
using Beskar.CodeAnalytics.Data.Indexes.Models;
using Beskar.CodeAnalytics.Data.Metadata.Indexes;
using Beskar.CodeAnalytics.Data.Metadata.Indexes.Cache;
using Beskar.CodeAnalytics.Data.Metadata.Specs;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Storage;
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
         FullPathName = CreateNGramIndex<SymbolSpec>(FileIds.Symbol, context, IndexNames.Symbol.FullPathName, x => x.FullPathName)
      };
      
      // Method Symbols
      context.DatabaseBuilder.Symbols.MethodIndexes = new MethodSymbolSpecDescriptor.Indexes()
      {
         ParameterCount = CreateBTreeIndex<MethodSymbolSpec, int>(FileIds.MethodSymbol,
            context, IndexNames.MethodSymbol.ParameterCount, x => x.Parameters.Count, 
            Comparer<KeyedIndexEntry<int>>.Create((x, y) => x.Key - y.Key))
      };
      
      // Folder
      context.DatabaseBuilder.Structure.FolderIndexes = new FolderSpecDescriptor.Indexes
      {
         ParentId = CreateBTreeIndex<FolderSpec, uint>(FileIds.Folder,
            context, IndexNames.Folder.ParentId, x => x.ParentId, 
            Comparer<KeyedIndexEntry<uint>>.Create((x, y) => x.Key.CompareTo(y.Key)))
      };
      
      return ValueTask.CompletedTask;
   }

   private BTreeIndexDescriptor<TKey> CreateBTreeIndex<TEntity, TKey>(
      FileId fileId,
      BakeContext context,
      string name, 
      Func<TEntity, TKey> selector,
      IComparer<KeyedIndexEntry<TKey>> comparer)
      where TEntity : unmanaged, ISpec
      where TKey : unmanaged, IComparable<TKey>
   {
      var storageFiles = context.DatabaseBuilder.Storage.Files;
      var result = new IndexBaker<TEntity, TKey>(selector, IndexType.StaticWideBTree, name, comparer).Bake(context);
      
      storageFiles.Add(new StorageFileDescriptor()
      {
         FileName = result.FileName,
         Name = name,
         ParentName = fileId.Value,
         LastModified = DateTimeOffset.UtcNow,
         Kind = StorageFileKind.Index,
         
         ByteCount = result.ByteCount,
         RowCount = result.RowCount
      });
      
      return new BTreeIndexDescriptor<TKey>()
      {
         Comparer = IndexComparerRegistry<TKey>.GetComparer(name),
         FileName = result.FileName,
         Name = name
      };
   }

   private NGramIndexDescriptor CreateNGramIndex<TEntity>(
      FileId fileId,
      BakeContext context,
      string name, 
      Func<TEntity, StringFileView> selector)
      where TEntity : unmanaged, ISpec
   {
      var storageFiles = context.DatabaseBuilder.Storage.Files;
      var result = new IndexBaker<TEntity, StringFileView>(selector, IndexType.NGram, name).Bake(context);
      
      storageFiles.Add(new StorageFileDescriptor()
      {
         FileName = result.FileName,
         Name = name,
         ParentName = fileId.Value,
         LastModified = DateTimeOffset.UtcNow,
         Kind = StorageFileKind.Index,
         
         ByteCount = result.ByteCount,
         RowCount = result.RowCount
      });
      
      return new NGramIndexDescriptor()
      {
         FileName = result.FileName,
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