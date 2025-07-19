using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Types;

namespace CodeAnalytics.Engine.Contracts.Archetypes.Types;

[StructLayout(LayoutKind.Auto)]
public struct InterfaceArchetype 
   : IEquatable<InterfaceArchetype>
{
   public SymbolComponent Symbol;
   public TypeComponent Type;
   public InterfaceComponent Interface;

   public bool Equals(InterfaceArchetype other)
   {
      return Symbol.Equals(other.Symbol) && Type.Equals(other.Type) && Interface.Equals(other.Interface);
   }

   public override bool Equals(object? obj)
   {
      return obj is InterfaceArchetype other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Symbol, Type, Interface);
   }

   public static bool operator ==(InterfaceArchetype left, InterfaceArchetype right)
   {
     return left.Equals(right);
   }

   public static bool operator !=(InterfaceArchetype left, InterfaceArchetype right)
   {
     return !(left == right);
   }
}