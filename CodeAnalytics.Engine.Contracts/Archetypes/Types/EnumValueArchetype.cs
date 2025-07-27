using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Archetypes.Types;

[StructLayout(LayoutKind.Auto)]
public struct EnumValueArchetype
   : IEquatable<EnumValueArchetype>, IArchetype
{
   public NodeId NodeId => Symbol.Id;
   public SymbolComponent SymbolComponent => Symbol;
   
   public SymbolComponent Symbol;
   public EnumValueComponent EnumValue;
   
   public void Dispose()
   {
      Symbol.Dispose();
      EnumValue.Dispose();
   }


   public bool Equals(EnumValueArchetype other)
   {
      return Symbol.Equals(other.Symbol) && EnumValue.Equals(other.EnumValue);
   }

   public override bool Equals(object? obj)
   {
      return obj is EnumValueArchetype other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Symbol, EnumValue);
   }

   public static bool operator ==(EnumValueArchetype left, EnumValueArchetype right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(EnumValueArchetype left, EnumValueArchetype right)
   {
      return !(left == right);
   }
}