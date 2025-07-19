using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Components.Inerfaces;
using CodeAnalytics.Engine.Contracts.Enums.Symbols;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Components.Members;

[StructLayout(LayoutKind.Auto)]
public struct MemberComponent 
   : IEquatable<MemberComponent>, IComponent
{
   public NodeId NodeId => Id;
   public NodeId Id = NodeId.Empty;
   
   public AccessModifier Access;
   public PackedBools Flags;

   public bool IsStatic
   {
      get => Flags.Get(IsStaticIndex);
      set => Flags.Set(IsStaticIndex, value);
   }

   public NodeId MemberTypeId = NodeId.Empty;
   public NodeId ContainingTypeId = NodeId.Empty;
   
   public PooledSet<NodeId> AttributeIds = [];

   public MemberComponent()
   {
      
   }
   
   private const int IsStaticIndex = 0;
   
   public bool Equals(MemberComponent other)
   {
      return Id.Equals(other.Id);
   }

   public override bool Equals(object? obj)
   {
      return obj is MemberComponent other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Id);
   }

   public static bool operator ==(MemberComponent left, MemberComponent right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(MemberComponent left, MemberComponent right)
   {
      return !(left == right);
   }
}