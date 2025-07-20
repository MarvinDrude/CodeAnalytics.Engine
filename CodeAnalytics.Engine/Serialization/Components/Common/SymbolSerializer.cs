using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Collections;
using CodeAnalytics.Engine.Serialization.Ids;

namespace CodeAnalytics.Engine.Serialization.Components.Common;

public sealed class SymbolSerializer : ISerializer<SymbolComponent>
{
   public static void Serialize(ref ByteWriter writer, ref SymbolComponent ob)
   {
      NodeIdSerializer.Serialize(ref writer, ref ob.Id);
      
      StringIdSerializer.Serialize(ref writer, ref ob.Name);
      StringIdSerializer.Serialize(ref writer, ref ob.MetadataName);
      StringIdSerializer.Serialize(ref writer, ref ob.FullPathName);
      
      PooledSetSerializer<StringId, StringIdSerializer>.Serialize(ref writer, ref ob.FileLocations);
      PooledSetSerializer<StringId, StringIdSerializer>.Serialize(ref writer, ref ob.Projects);
   }

   public static bool TryDeserialize(ref ByteReader reader, out SymbolComponent ob)
   {
      ob = new SymbolComponent();
      
      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.Id))
      {
         return false;
      }

      if (!StringIdSerializer.TryDeserialize(ref reader, out ob.Name)
          || !StringIdSerializer.TryDeserialize(ref reader, out ob.MetadataName)
          || !StringIdSerializer.TryDeserialize(ref reader, out ob.FullPathName))
      {
         return false;
      }

      if (!PooledSetSerializer<StringId, StringIdSerializer>.TryDeserialize(ref reader, out ob.FileLocations)
          || !PooledSetSerializer<StringId, StringIdSerializer>.TryDeserialize(ref reader, out ob.Projects))
      {
         return false;
      }
      
      return true;
   }
}