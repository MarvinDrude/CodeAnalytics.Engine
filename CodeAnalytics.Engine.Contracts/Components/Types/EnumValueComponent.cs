using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Components.Inerfaces;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Components.Types;

[StructLayout(LayoutKind.Auto)]
public struct EnumValueComponent 
   : IComponent, IEquatable<EnumValueComponent>
{
   public NodeId NodeId => Id;
   public NodeId Id = NodeId.Empty;
   public PackedBools Flags;

   public StringId Name = StringId.Empty;
   public long Value;
   public ulong UValue;

   public bool IsULong
   {
      get => Flags.Get(IsULongIndex);
      set => Flags.Set(IsULongIndex, value);
   }
   
   public EnumValueComponent()
   {
   }

   private const int IsULongIndex = 0;
   
   public void Dispose()
   {
   }

   public bool Equals(EnumValueComponent other)
   {
      return Id.Equals(other.Id);
   }

   public override bool Equals(object? obj)
   {
      return obj is EnumValueComponent other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Id);
   }

   public static bool operator ==(EnumValueComponent left, EnumValueComponent right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(EnumValueComponent left, EnumValueComponent right)
   {
      return !(left == right);
   }
}