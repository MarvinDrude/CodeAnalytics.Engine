using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Archetypes.Members;

[StructLayout(LayoutKind.Auto)]
public struct PropertyArchetype 
   : IEquatable<PropertyArchetype>, IArchetype
{
   public NodeId NodeId => Symbol.Id;
   public SymbolComponent SymbolComponent => Symbol;
   
   public SymbolComponent Symbol;
   public MemberComponent Member;
   public PropertyComponent Property;

   public void Dispose()
   {
      Symbol.Dispose();
      Member.Dispose();
      Property.Dispose();
   }
   
   public bool Equals(PropertyArchetype other)
   {
      return Symbol.Equals(other.Symbol) && Member.Equals(other.Member) && Property.Equals(other.Property);
   }

   public override bool Equals(object? obj)
   {
      return obj is PropertyArchetype other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Symbol, Member, Property);
   }

   public static bool operator ==(PropertyArchetype left, PropertyArchetype right)
   {
     return left.Equals(right);
   }

   public static bool operator !=(PropertyArchetype left, PropertyArchetype right)
   {
     return !(left == right);
   }
}