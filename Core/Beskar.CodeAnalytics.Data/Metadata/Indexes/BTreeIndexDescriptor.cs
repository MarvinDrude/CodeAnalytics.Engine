using Beskar.CodeAnalytics.Data.Enums.Indexes;
using Beskar.CodeAnalytics.Data.Indexes.Readers;

namespace Beskar.CodeAnalytics.Data.Metadata.Indexes;

public sealed class BTreeIndexDescriptor<TKey> : IndexDescriptor<TKey>
    where TKey : unmanaged, IComparable<TKey>
{
    public override IndexType Type => IndexType.StaticWideBTree;
    
    public required IComparer<TKey> Comparer { get; set; }
    
    public BTreeIndexReader<TKey> Reader => field ??= new BTreeIndexReader<TKey>(
        _filePath ?? throw new InvalidOperationException("File path not initialized"), Comparer);

    public override void Dispose()
    {
        if (!_initialized) return;
        Reader.Dispose();
    }
}