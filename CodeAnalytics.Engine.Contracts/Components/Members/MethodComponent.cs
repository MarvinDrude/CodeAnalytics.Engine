using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Components.Members;

[StructLayout(LayoutKind.Auto)]
public struct MethodComponent 
   : IEquatable<MethodComponent>
{
   public NodeId Id = NodeId.Empty;
   public PackedBools Flags;

   public bool IsAsync
   {
      get => Flags.Get(IsAsyncIndex);
      set => Flags.Set(IsAsyncIndex, value);
   }

   public bool IsAbstract
   {
      get => Flags.Get(IsAbstractIndex);
      set => Flags.Set(IsAbstractIndex, value);
   }

   public bool IsOverride
   {
      get => Flags.Get(IsOverrideIndex);
      set => Flags.Set(IsOverrideIndex, value);
   }

   public int CyclomaticComplexity;
   
   public MethodComponent()
   {
      
   }

   private const int IsAsyncIndex = 0;
   private const int IsAbstractIndex = 1;
   private const int IsOverrideIndex = 2;
   
   public bool Equals(MethodComponent other)
   {
      return Id.Equals(other.Id);
   }

   public override bool Equals(object? obj)
   {
      return obj is MethodComponent other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Id);
   }

   public static bool operator ==(MethodComponent left, MethodComponent right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(MethodComponent left, MethodComponent right)
   {
      return !(left == right);
   }
}