using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Components.Members;

[StructLayout(LayoutKind.Auto)]
public struct ConstructorComponent 
   : IEquatable<ConstructorComponent>
{
   public NodeId Id = NodeId.Empty;
   
   public ConstructorComponent()
   {
      
   }
   
   public bool Equals(ConstructorComponent other)
   {
      return Id.Equals(other.Id);
   }

   public override bool Equals(object? obj)
   {
      return obj is ConstructorComponent other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Id);
   }

   public static bool operator ==(ConstructorComponent left, ConstructorComponent right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(ConstructorComponent left, ConstructorComponent right)
   {
      return !(left == right);
   }
}