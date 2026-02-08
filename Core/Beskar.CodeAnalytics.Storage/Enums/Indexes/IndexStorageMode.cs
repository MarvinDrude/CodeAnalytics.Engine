namespace Beskar.CodeAnalytics.Storage.Enums.Indexes;

public enum IndexStorageMode : byte
{
   /// <summary>
   /// Stores [Key, SymbolId]
   /// </summary>
   Dense = 1,
   /// <summary>
   /// Stores only [SymbolId], key is purely in the header
   /// </summary>
   Sparse = 2
}