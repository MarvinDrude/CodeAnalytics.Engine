using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Components.Types;

[StructLayout(LayoutKind.Auto)]
public struct StructComponent
   : IEquatable<StructComponent>
{
   public NodeId Id = NodeId.Empty;
   public PackedBools Flags;

   public bool IsRef
   {
      get => Flags.Get(IsRefIndex);
      set => Flags.Set(IsRefIndex, value);
   }

   public bool IsReadOnly
   {
      get => Flags.Get(IsReadOnlyIndex);
      set => Flags.Set(IsReadOnlyIndex, value);
   }
   
   public StructComponent()
   {
      
   }

   private const int IsRefIndex = 0;
   private const int IsReadOnlyIndex = 1;
   
   public bool Equals(StructComponent other)
   {
      return Id.Equals(other.Id);
   }

   public override bool Equals(object? obj)
   {
      return obj is StructComponent other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Id);
   }

   public static bool operator ==(StructComponent left, StructComponent right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(StructComponent left, StructComponent right)
   {
      return !(left == right);
   }
}