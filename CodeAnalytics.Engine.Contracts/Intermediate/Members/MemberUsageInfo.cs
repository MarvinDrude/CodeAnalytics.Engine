using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Enums.Intermediate;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Intermediate.Members;

[StructLayout(LayoutKind.Auto)]
public struct MemberUsageInfo 
   : IEquatable<MemberUsageInfo>
{
   public NodeId ContainingId = NodeId.Empty;
   public NodeId MemberId =  NodeId.Empty;
   
   public MemberUsageType Type;
   public int LoopScore;

   public PackedBools Flags;

   public bool IsStatic
   {
      get => Flags.Get(IsStaticIndex);
      set => Flags.Set(IsStaticIndex, value);
   }

   public MemberUsageInfo()
   {
      
   }
   
   private const int IsStaticIndex = 0;

   public bool Equals(MemberUsageInfo other)
   {
      return ContainingId.Equals(other.ContainingId) && MemberId.Equals(other.MemberId);
   }

   public override bool Equals(object? obj)
   {
      return obj is MemberUsageInfo other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(ContainingId, MemberId);
   }

   public static bool operator ==(MemberUsageInfo left, MemberUsageInfo right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(MemberUsageInfo left, MemberUsageInfo right)
   {
      return !(left == right);
   }
}