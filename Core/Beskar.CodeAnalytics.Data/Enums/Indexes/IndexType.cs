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
   /// Bake Steps (Low memory usage):
   /// - Stream source file, write temporary unordered file (Key, ID)
   /// - External Multi-File sort (all keys are physically next to each other now)
   /// - Read sorted file, for each unique key, write ids to contigous block in data temp file
   /// - Simultaneously write Dictionary entries (Key, DataOffset, Count) to dict temp file
   /// - Stitch final file together
   /// </summary>
   StaticGroupedInverted = 1,
   /// <summary>
   /// (Best for range scans or exact matches)
   /// Index File Setup:
   /// - Index Header
   /// - Leaf Level: Contiguous Data Blocks
   /// - Level 1 (Children): 1000 Pages
   /// - Level 0 (Root): 1 Page (e.g. 4KB, ~1000 keys)
   /// Bake Steps (Low memory usage):
   /// - Create sorted file by (Key, ID)
   /// - Leaf packing: stream sorted entries and write them into Pages (eg 4kb blocks) (only keep first key of every page in memory)
   /// - Internal Level Generation: Take the list of "First keys" from the leaf level (Thee become the entries for the next level up (Level 1))
   /// - Repeat until one single root page
   /// - Write pointer based system where header points to the root offset
   /// </summary>
   StaticWideBTree = 2,
   /// <summary>
   /// (Best for O(1) lookups of unique keys)
   /// - Buckets (Perfect Hash or Cuckoo Hash Layout):
   ///    - Fixed-size slots containing (HashKey, DataOffset)
   /// - Overflow/Collision Segment (if not using Perfect Hashing)
   /// - Data Segment: The actual values or ID lists
   /// Base Steps (Low memory usage):
   /// - Count & Allocate: First count the unique keys to calc good bucket count (0.7 load factor)
   /// - Bucket file reserve: create temp file with filled zero slots of size BucketCount * sizeof(slot)
   /// - Stream source data, calculate hash of key h = Hash(Key) % BucketCount
   /// - Seek to position in temp bucket file
   /// - If slot is full, use Linear probing until empty slot is found
   /// - Write (HashKey, DataOffset/ID) into slot
   /// - Final file: Header -> buckets -> data
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
   /// Bake Steps (Low memory usage):
   /// - Write Temporary unordered file of entries (NGram, Id)
   /// - External Multi-File sort (all NGrams entrie are physically next to each other now)
   /// - Stream the file (one key in memory at a time) and write grouped entry in postings segments in data temp file
   /// - Stream the dictionary data to dictionary temp file (while processing the grouping logic)
   /// - Combine the 2 temp files into final file
   /// </summary>
   NGram = 4
}