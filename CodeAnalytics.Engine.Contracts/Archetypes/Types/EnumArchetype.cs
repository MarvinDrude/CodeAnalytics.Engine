using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Archetypes.Types;

[StructLayout(LayoutKind.Auto)]
public struct EnumArchetype 
   : IEquatable<EnumArchetype>, IArchetype
{
   public NodeId NodeId => Symbol.Id;
   
   public SymbolComponent Symbol;
   public EnumComponent Enum;

   public void Dispose()
   {
      Symbol.Dispose();
      Enum.Dispose();
   }
   
   public bool Equals(EnumArchetype other)
   {
      return Symbol.Equals(other.Symbol) && Enum.Equals(other.Enum);
   }

   public override bool Equals(object? obj)
   {
      return obj is EnumArchetype other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Symbol, Enum);
   }

   public static bool operator ==(EnumArchetype left, EnumArchetype right)
   {
     return left.Equals(right);
   }

   public static bool operator !=(EnumArchetype left, EnumArchetype right)
   {
     return !(left == right);
   }
}