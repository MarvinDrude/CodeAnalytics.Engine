using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Archetypes.Types;

[StructLayout(LayoutKind.Auto)]
public struct ClassArchetype 
   : IEquatable<ClassArchetype>, IArchetype
{
   public NodeId NodeId => Symbol.Id;
   public SymbolComponent SymbolComponent => Symbol;
   
   public SymbolComponent Symbol;
   public TypeComponent Type;
   public ClassComponent Class;
   
   public void Dispose()
   {
      Symbol.Dispose();
      Type.Dispose();
      Class.Dispose();
   }
   
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