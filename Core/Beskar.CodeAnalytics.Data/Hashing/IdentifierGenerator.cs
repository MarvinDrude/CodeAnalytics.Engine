using System.Collections.Concurrent;
using Beskar.CodeAnalytics.Data.Entities.Misc;

namespace Beskar.CodeAnalytics.Data.Hashing;

public sealed class IdentifierGenerator
{
   private readonly ConcurrentDictionary<ulong, uint> _strOffsetToId = [];
   private readonly ConcurrentDictionary<uint, ulong> _idToStrOffset = [];

   public uint GenerateIdentifier(
      scoped in ReadOnlySpan<char> fullPathId, StringFileView view)
   {
      return GenerateIdentifier(fullPathId, view.Offset);
   }

   public uint GenerateIdentifier(
      scoped in ReadOnlySpan<char> fullPathId, ulong strOffset)
   {
      if (_strOffsetToId.TryGetValue(strOffset, out var existingId))
      {
         return existingId;
      }

      var id = FastHasher32.GetDeterministicId(fullPathId);
      while (true)
      {
         if (_idToStrOffset.TryAdd(id, strOffset))
         {
            _strOffsetToId[strOffset] = id;
            return id;
         }

         if (_idToStrOffset[id] == strOffset)
         {
            return id;
         }

         var step = FastHasher32.GetDeterministicId(fullPathId, 900_000);
         unchecked
         {
            id += step;
         }
      }
   }
}