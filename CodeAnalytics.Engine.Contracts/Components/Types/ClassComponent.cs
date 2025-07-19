using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Components.Inerfaces;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Components.Types;

[StructLayout(LayoutKind.Auto)]
public struct ClassComponent
   : IEquatable<ClassComponent>, IComponent
{
   public NodeId NodeId => Id;
   public NodeId Id = NodeId.Empty;
   public PackedBools Flags;

   public bool IsStatic
   {
      get => Flags.Get(IsStaticIndex);
      set => Flags.Set(IsStaticIndex, value);
   }

   public bool IsAbstract
   {
      get => Flags.Get(IsAbstractIndex);
      set => Flags.Set(IsAbstractIndex, value);
   }

   public bool IsSealed
   {
      get => Flags.Get(IsSealedIndex);
      set => Flags.Set(IsSealedIndex, value);
   }

   public NodeId BaseClassId = NodeId.Empty;
   
   public ClassComponent()
   {
      
   }
   
   private const int IsStaticIndex = 0;
   private const int IsAbstractIndex = 1;
   private const int IsSealedIndex = 2;
   
   public void Dispose()
   {
      
   }
   
   public bool Equals(ClassComponent other)
   {
      return Id.Equals(other.Id);
   }

   public override bool Equals(object? obj)
   {
      return obj is ClassComponent other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Id);
   }

   public static bool operator ==(ClassComponent left, ClassComponent right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(ClassComponent left, ClassComponent right)
   {
      return !(left == right);
   }
}