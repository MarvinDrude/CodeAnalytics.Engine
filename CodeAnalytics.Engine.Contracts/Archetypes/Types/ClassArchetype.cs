using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Types;

namespace CodeAnalytics.Engine.Contracts.Archetypes.Types;

[StructLayout(LayoutKind.Auto)]
public struct ClassArchetype 
   : IEquatable<ClassArchetype>
{
   public SymbolComponent Symbol;
   public TypeComponent Type;
   public ClassComponent Class;
   
   public bool Equals(ClassArchetype other)
   {
      return Symbol.Equals(other.Symbol) && Type.Equals(other.Type) && Class.Equals(other.Class);
   }

   public override bool Equals(object? obj)
   {
      return obj is ClassArchetype other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Symbol, Type, Class);
   }

   public static bool operator ==(ClassArchetype left, ClassArchetype right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(ClassArchetype left, ClassArchetype right)
   {
      return !(left == right);
   }
}