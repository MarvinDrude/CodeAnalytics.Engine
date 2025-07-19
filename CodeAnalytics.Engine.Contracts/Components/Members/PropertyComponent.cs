using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Components.Inerfaces;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Components.Members;

[StructLayout(LayoutKind.Auto)]
public struct PropertyComponent 
   : IEquatable<PropertyComponent>, IComponent
{
   public NodeId NodeId => Id;
   public NodeId Id = NodeId.Empty;
   public PackedBools Flags;

   public bool HasGetter
   {
      get => Flags.Get(HasGetterIndex);
      set => Flags.Set(HasGetterIndex, value);
   }

   public bool HasSetter
   {
      get => Flags.Get(HasSetterIndex);
      set => Flags.Set(HasSetterIndex, value);
   }

   public PropertyComponent()
   {
      
   }

   private const int HasGetterIndex = 0;
   private const int HasSetterIndex = 1;
   
   public void Dispose()
   {
      
   }
   
   public bool Equals(PropertyComponent other)
   {
      return Id.Equals(other.Id);
   }

   public override bool Equals(object? obj)
   {
      return obj is PropertyComponent other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Id);
   }

   public static bool operator ==(PropertyComponent left, PropertyComponent right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(PropertyComponent left, PropertyComponent right)
   {
      return !(left == right);
   }
}