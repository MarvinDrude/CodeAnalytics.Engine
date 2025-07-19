using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Components.Types;

[StructLayout(LayoutKind.Auto)]
public struct InterfaceComponent
   : IEquatable<InterfaceComponent>
{
   public NodeId Id = NodeId.Empty;
   
   public InterfaceComponent()
   {
      
   }
   
   public bool Equals(InterfaceComponent other)
   {
      return Id.Equals(other.Id);
   }

   public override bool Equals(object? obj)
   {
      return obj is InterfaceComponent other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Id);
   }

   public static bool operator ==(InterfaceComponent left, InterfaceComponent right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(InterfaceComponent left, InterfaceComponent right)
   {
      return !(left == right);
   }
}