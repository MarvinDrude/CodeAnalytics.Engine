using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Components.Members;

[StructLayout(LayoutKind.Auto)]
public struct FieldComponent 
   : IEquatable<FieldComponent>
{
   public NodeId Id = NodeId.Empty;
   public PackedBools Flags;

   public bool IsConst
   {
      get => Flags.Get(IsConstIndex);
      set => Flags.Set(IsConstIndex, value);
   }

   public bool IsReadOnly
   {
      get => Flags.Get(IsReadOnlyIndex);
      set => Flags.Set(IsReadOnlyIndex, value);
   }

   public FieldComponent()
   {
      
   }
   
   private const int IsConstIndex = 0;
   private const int IsReadOnlyIndex = 1;
   
   public bool Equals(FieldComponent other)
   {
      return Id.Equals(other.Id);
   }

   public override bool Equals(object? obj)
   {
      return obj is FieldComponent other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Id);
   }

   public static bool operator ==(FieldComponent left, FieldComponent right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(FieldComponent left, FieldComponent right)
   {
      return !(left == right);
   }
}