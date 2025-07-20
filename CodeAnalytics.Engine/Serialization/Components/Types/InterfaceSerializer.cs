using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Ids;

namespace CodeAnalytics.Engine.Serialization.Components.Types;

public sealed class InterfaceSerializer : ISerializer<InterfaceComponent>
{
   public static void Serialize(ref ByteWriter writer, ref InterfaceComponent ob)
   {
      NodeIdSerializer.Serialize(ref writer, ref ob.Id);
   }

   public static bool TryDeserialize(ref ByteReader reader, out InterfaceComponent ob)
   {
      ob = new InterfaceComponent();
      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.Id))
      {
         return false;
      }
      
      return true;
   }
}