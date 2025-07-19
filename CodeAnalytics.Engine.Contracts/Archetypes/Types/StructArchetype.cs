using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Archetypes.Types;

[StructLayout(LayoutKind.Auto)]
public struct StructArchetype 
   : IEquatable<StructArchetype>, IArchetype
{
   public NodeId NodeId => Symbol.Id;
   
   public SymbolComponent Symbol;
   public TypeComponent Type;
   public StructComponent Struct;

   public void Dispose()
   {
      Symbol.Dispose();
      Type.Dispose();
      Struct.Dispose();
   }
   
   public bool Equals(StructArchetype other)
   {
      return Symbol.Equals(other.Symbol) && Type.Equals(other.Type) && Struct.Equals(other.Struct);
   }

   public override bool Equals(object? obj)
   {
      return obj is StructArchetype other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Symbol, Type, Struct);
   }

   public static bool operator ==(StructArchetype left, StructArchetype right)
   {
     return left.Equals(right);
   }

   public static bool operator !=(StructArchetype left, StructArchetype right)
   {
     return !(left == right);
   }
}