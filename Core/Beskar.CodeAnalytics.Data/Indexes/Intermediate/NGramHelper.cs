using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;
using Beskar.CodeAnalytics.Data.Indexes.Models;
using Me.Memory.Buffers;

namespace Beskar.CodeAnalytics.Data.Indexes.Intermediate;

/// <summary>
/// Provides helper functionality for generating N-Grams from input sequences.
/// </summary>
public static class NGramHelper
{
   public static KeyedIndexEntry<T>[] CreateNGrams<T>(
      scoped in ReadOnlySpan<char> input, uint id, int n,
      bool addPadding = true)
      where T : unmanaged
   {
      // Good estimate on how big the array capacity needs to be
      var expectedCount = GetEstimatedNGramCount(input, n);
      using var arrayBuilder = new ArrayBuilder<KeyedIndexEntry<T>>(expectedCount);
      
      // Run the actual algortihm + return a new array that is not rented
      // (since we don't know what the caller wants to do with it)
      CreateNGrams(arrayBuilder, input, id, n, addPadding);
      return arrayBuilder.WrittenSpan.ToArray();
   }

   /// <summary>
   /// Estimates the count of N-Grams that can be generated from the given input string and N-Gram length.
   /// </summary>
   /// <param name="input">The input string to process, represented as a <see cref="ReadOnlySpan{Char}"/>.</param>
   /// <param name="n">The length of the N-Gram to generate.</param>
   /// <returns>The estimated number of N-Grams that can be generated.</returns>
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static int GetEstimatedNGramCount(scoped in ReadOnlySpan<char> input, int n)
   {
      // we accept that this is bigger than needed if we have for example
      // a surrogate emoji of multiple emoji parts
      var expectedCount = input.Length + n - 1;
      return expectedCount;
   }
   
   public static unsafe void CreateNGrams<T>(
      ArrayBuilder<KeyedIndexEntry<T>> resultBuilder,
      scoped in ReadOnlySpan<char> input, uint id, int n,
      bool addPadding = true)
      where T : unmanaged
   {
      // make sure the T is always a valid memory layout
      if (typeof(T) != typeof(NGram3)
          && typeof(T) != typeof(NGram4))
      {
         throw new NotSupportedException($"NGrams of type {typeof(T).Name} are not supported.");
      }
      
      // lengths and padding
      var paddingCount = addPadding ? n - 1 : 0;
      // padding is 1 byte per char in utf8 when using dollar sign
      var inputByteCount = Encoding.UTF8.GetByteCount(input);
      var maxByteCount = inputByteCount + (paddingCount * 2);
      
      // rented buffer
      using var byteBufferOwner = new SpanOwner<byte>(maxByteCount);
      
      // UTF-8 for '$' is 0x24.
      if (addPadding)
      {
         byteBufferOwner.Span[..paddingCount].Fill((byte)'\u0002');
      }
      var written = Encoding.UTF8.GetBytes(input, byteBufferOwner.Span.Slice(paddingCount, inputByteCount));
      
      // UTF-8 for '$' is 0x24.
      if (addPadding)
      {
         byteBufferOwner.Span[(paddingCount + written)..].Fill((byte)'\u0002');
      }

      // make sure we only stack alloc small sizes
      // bound size is the upper bound, utf8 will use less or equal to that
      var boundSize = maxByteCount + 1;
      using var boundsOwner = boundSize <= 128 
         ? new SpanOwner<int>(stackalloc int[boundSize])
         : new SpanOwner<int>(boundSize);
         
      var charCount = 0;

      // We also split emoji sequences into single points (by design)
      for (var e = 0; e < maxByteCount; e++)
      {
         // start or continuation byte check
         // Are the first two bits of this byte not equal to 10?
         if ((byteBufferOwner.Span[e] & 0xC0) != 0x80)
         {
            boundsOwner.Span[charCount++] = e;
         }
      }
      boundsOwner.Span[charCount] = maxByteCount;

      var entryCount = charCount - n + 1;
      // structs are sequential + pack = 1 to make sure
      var maxDataCapacity = sizeof(T) - 1;
      
      // iterate all engram ranges
      for (var e = 0; e < entryCount; e++)
      {
         var start = boundsOwner.Span[e];
         var end = boundsOwner.Span[e + n];
         
         // make sure we do not accidently buffer overflow
         var length = end - start;
         // should never happen - we have 17 bytes for NGram4, 13 bytes for NGram3
         // and we split emoji sequences
         ArgumentOutOfRangeException.ThrowIfGreaterThan(length, maxDataCapacity);
         
         // Offset 0: Length, Offset 1: Data
         T gram = default;
         var pGram = (byte*)&gram;
         
         // First byte is the acutal length used of the fixed byte array following
         *pGram = (byte)length;
         // Copy the bytes used into the fixed byte array of the struct
         byteBufferOwner.Span.Slice(start, length).CopyTo(new Span<byte>(pGram + 1, length));
            
         resultBuilder.Add(new KeyedIndexEntry<T>()
         {
            Id = id,
            Key = gram
         });
      }
   }
}