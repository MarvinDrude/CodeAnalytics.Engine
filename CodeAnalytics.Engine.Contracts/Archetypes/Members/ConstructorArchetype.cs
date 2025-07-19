using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Members;

namespace CodeAnalytics.Engine.Contracts.Archetypes.Members;

[StructLayout(LayoutKind.Auto)]
public struct ConstructorArchetype 
   : IEquatable<ConstructorArchetype>
{
   public SymbolComponent Symbol;
   public MemberComponent Member;
   public ConstructorComponent Constructor;

   public bool Equals(ConstructorArchetype other)
   {
      return Symbol.Equals(other.Symbol) && Member.Equals(other.Member) && Constructor.Equals(other.Constructor);
   }

   public override bool Equals(object? obj)
   {
      return obj is ConstructorArchetype other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Symbol, Member, Constructor);
   }

   public static bool operator ==(ConstructorArchetype left, ConstructorArchetype right)
   {
     return left.Equals(right);
   }

   public static bool operator !=(ConstructorArchetype left, ConstructorArchetype right)
   {
     return !(left == right);
   }
}