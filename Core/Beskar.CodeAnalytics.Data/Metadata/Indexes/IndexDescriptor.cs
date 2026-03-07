using Beskar.CodeAnalytics.Data.Enums.Indexes;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Indexes;

public abstract class IndexDescriptor<TKey> : IIndexDescriptor
   where TKey : unmanaged, IComparable<TKey>
{
   public required string Name { get; init; }
   
   public required string FileName { get; init; }
   
   public abstract IndexType Type { get; }
   
   protected string? _filePath;
   protected bool _initialized;

   public virtual void Initialize(DatabaseDescriptor database)
   {
      _filePath = Path.Combine(database.BaseFolderPath, FileName);
      _initialized = true;
   }
   
   public abstract void Dispose();
}