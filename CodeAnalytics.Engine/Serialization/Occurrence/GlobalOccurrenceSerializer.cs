using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Occurrences;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Ids;
using CodeAnalytics.Engine.Serialization.System.Collections;

namespace CodeAnalytics.Engine.Serialization.Occurrence;

public sealed class GlobalOccurrenceSerializer : ISerializer<GlobalOccurrence>
{
   public static void Serialize(ref ByteWriter writer, ref GlobalOccurrence ob)
   {
      var nodeId = ob.NodeId;
      NodeIdSerializer.Serialize(ref writer, ref nodeId);
      
      var dict = ob.ToDictionary();
      DictionarySerializer<StringId, StringIdSerializer, ProjectOccurrence, ProjectOccurrenceSerializer>
         .Serialize(ref writer, ref dict);

      var declarations = ob.Declarations;
      ListSerializer<DeclarationOccurrence, DeclarationOccurrenceSerializer>
         .Serialize(ref writer, ref declarations);
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out GlobalOccurrence ob)
   {
      ob = null;

      if (!NodeIdSerializer.TryDeserialize(ref reader, out var nodeId))
      {
         return false;
      }
      
      if (!DictionarySerializer<StringId, StringIdSerializer, ProjectOccurrence, ProjectOccurrenceSerializer>
             .TryDeserialize(ref reader, out var dict))
      {
         return false;
      }

      if (!ListSerializer<DeclarationOccurrence, DeclarationOccurrenceSerializer>
             .TryDeserialize(ref reader, out var declarations))
      {
         return false;
      }

      ob = new GlobalOccurrence()
      {
         NodeId = nodeId,
      };

      foreach (var declaration in declarations)
      {
         ob.AddDeclaration(declaration);
      }

      foreach (var (key, value) in dict)
      {
         ob.ProjectOccurrences[key] = value;
      }
      
      return true;
   }
}