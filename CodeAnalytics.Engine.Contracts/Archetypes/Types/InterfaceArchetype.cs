using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Archetypes.Types;

[StructLayout(LayoutKind.Auto)]
public struct InterfaceArchetype 
   : IEquatable<InterfaceArchetype>, IArchetype
{
   public NodeId NodeId => Symbol.Id;
   
   public SymbolComponent Symbol;
   public TypeComponent Type;
   public InterfaceComponent Interface;

   public void Dispose()
   {
      Symbol.Dispose();
      Type.Dispose();
      Interface.Dispose();
   }
   
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