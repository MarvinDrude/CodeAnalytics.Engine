
using Beskar.CodeAnalytics.Data.Enums.Indexes;
using Beskar.CodeAnalytics.Data.Indexes.Readers;

namespace Beskar.CodeAnalytics.Data.Metadata.Indexes;

public sealed class NGramIndexDescriptor : IndexDescriptor<uint>
{
   public override IndexType Type => IndexType.NGram;
   
   public NGramIndexReader Reader => field ??= new NGramIndexReader(
      _filePath ?? throw new InvalidOperationException("File path not initialized"));
   
   public override void Dispose()
   {
      if (!_initialized) return;
      Reader.Dispose();
   }
}