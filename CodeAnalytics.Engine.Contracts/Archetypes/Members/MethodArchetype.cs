using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Members;

namespace CodeAnalytics.Engine.Contracts.Archetypes.Members;

[StructLayout(LayoutKind.Auto)]
public struct MethodArchetype 
   : IEquatable<MethodArchetype>
{
   public SymbolComponent Symbol;
   public MemberComponent Member;
   public MethodComponent Method;

   public bool Equals(MethodArchetype other)
   {
      return Symbol.Equals(other.Symbol) && Member.Equals(other.Member) && Method.Equals(other.Method);
   }

   public override bool Equals(object? obj)
   {
      return obj is MethodArchetype other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Symbol, Member, Method);
   }

   public static bool operator ==(MethodArchetype left, MethodArchetype right)
   {
     return left.Equals(right);
   }

   public static bool operator !=(MethodArchetype left, MethodArchetype right)
   {
     return !(left == right);
   }
}