using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Members;

namespace CodeAnalytics.Engine.Contracts.Archetypes.Members;

[StructLayout(LayoutKind.Auto)]
public struct FieldArchetype 
   : IEquatable<FieldArchetype>, IArchetype
{
   public SymbolComponent Symbol;
   public MemberComponent Member;
   public FieldComponent Field;

   public bool Equals(FieldArchetype other)
   {
      return Symbol.Equals(other.Symbol) && Member.Equals(other.Member) && Field.Equals(other.Field);
   }

   public override bool Equals(object? obj)
   {
      return obj is FieldArchetype other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Symbol, Member, Field);
   }

   public static bool operator ==(FieldArchetype left, FieldArchetype right)
   {
     return left.Equals(right);
   }

   public static bool operator !=(FieldArchetype left, FieldArchetype right)
   {
     return !(left == right);
   }
}