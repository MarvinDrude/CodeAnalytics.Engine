using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Components.Inerfaces;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Components.Types;

[StructLayout(LayoutKind.Auto)]
public struct EnumComponent
   : IEquatable<EnumComponent>, IComponent
{
   public NodeId NodeId => Id;
   public NodeId Id = NodeId.Empty;
   public NodeId UnderlyingTypeId = NodeId.Empty;

   public PooledSet<NodeId> ValueIds = [];
   
   public EnumComponent()
   {
      
   }
   
   public void Dispose()
   {
      ValueIds.Dispose();
   }
   
   public bool Equals(EnumComponent other)
   {
      return Id.Equals(other.Id);
   }

   public override bool Equals(object? obj)
   {
      return obj is EnumComponent other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Id);
   }

   public static bool operator ==(EnumComponent left, EnumComponent right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(EnumComponent left, EnumComponent right)
   {
      return !(left == right);
   }
}