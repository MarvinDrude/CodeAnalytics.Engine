using Beskar.CodeAnalytics.Data.Enums.Indexes;

namespace Beskar.CodeAnalytics.Data.Metadata.Indexes;

public interface IIndexDescriptor : IDisposable
{
   public string Name { get; }
   
   public string FileName { get; }
   
   public IndexType Type { get; }
}