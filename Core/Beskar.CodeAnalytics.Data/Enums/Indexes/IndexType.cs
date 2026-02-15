namespace Beskar.CodeAnalytics.Data.Enums.Indexes;

/// <summary>
/// In our scenario all indexes are bake once -> read only forever
/// which gives us some less complexity in scenarios like a b tree
/// </summary>
public enum IndexType : byte
{
   /// <summary>
   /// (Best for low cardinality data)
   /// Index File Setup:
   /// - Index Header
   /// - Dictionary Segment
   ///    - Mapping of what key starts where in the data segment
   ///    - [Key] => (Offset, ItemCount)
   /// - Data Segment
   ///    - Contigous array of IDs
   /// </summary>
   StaticGroupedInverted = 1,
   /// <summary>
   /// (Best for range scans or exact matches)
   /// Index File Setup:
   /// - Index Header
   /// - Leaf Level: Contiguous Data Blocks
   /// - Level 1 (Children): 1000 Pages
   /// - Level 0 (Root): 1 Page (e.g. 4KB, ~1000 keys)
   /// </summary>
   StaticWideBTree = 2,
   /// <summary>
   /// (Best for O(1) lookups of unique keys)
   /// - Buckets (Perfect Hash or Cuckoo Hash Layout):
   ///    - Fixed-size slots containing (HashKey, DataOffset)
   /// - Overflow/Collision Segment (if not using Perfect Hashing)
   /// - Data Segment: The actual values or ID lists
   /// </summary>
   Hash = 3,
   /// <summary>
   /// (Best for fuzzy search, autocomplete, or substring matching)
   /// Index File Setup:
   /// - Index Header
   /// - N-Gram Dictionary:
   ///    - Sorted list of N-character sequences (e.g., "abc", "bcd")
   ///    - [Gram] => Pointer to Postings List
   /// - Postings Segment:
   ///    - Contigous array of IDs
   /// </summary>
   NGram = 4
}