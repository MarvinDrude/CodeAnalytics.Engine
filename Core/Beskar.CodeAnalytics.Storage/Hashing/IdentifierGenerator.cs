using System;
using System.Collections.Generic;
using System.Threading;
using Beskar.CodeAnalytics.Storage.Entities.Misc;

namespace Beskar.CodeAnalytics.Storage.Hashing;

public sealed class IdentifierGenerator
{
   private readonly Lock _lock = new ();
   
   private readonly Dictionary<ulong, ulong> _identifiersToValue = [];
   private readonly Dictionary<ulong, ulong> _valueToIdentifier = [];
   
   public ulong GetDeterministicId(
      scoped in ReadOnlySpan<char> fullPath,
      scoped in StringDefinition stringDefinition)
   {
      lock (_lock)
      {
         if (_valueToIdentifier.TryGetValue(stringDefinition.Offset, out var existingId))
         {
            return existingId;
         }

         var id = DeterministicHasher.GetDeterministicId(fullPath);
         while (_identifiersToValue.TryGetValue(id, out var exitingFullPath))
         {
            if (exitingFullPath == stringDefinition.Offset) break;
            var step = GetFallbackHash(fullPath);

            unchecked
            {
               id += step;
            }
         }

         _valueToIdentifier.TryAdd(stringDefinition.Offset, id);
         _identifiersToValue[id] = stringDefinition.Offset;
         
         return id;
      }
   }

   private static ulong GetFallbackHash(scoped in ReadOnlySpan<char> fullPath)
   {
      // ensure the step is odd
      return DeterministicHasher.GetDeterministicId(fullPath, 900_000) | 1;
   }
}