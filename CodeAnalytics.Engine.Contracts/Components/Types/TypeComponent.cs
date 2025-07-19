using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Components.Inerfaces;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Components.Types;

[StructLayout(LayoutKind.Auto)]
public struct TypeComponent 
   : IEquatable<TypeComponent>, IComponent
{
   public NodeId NodeId => Id;
   public NodeId Id = NodeId.Empty;
   
   public PooledSet<NodeId> DirectInterfaceIds = [];
   public PooledSet<NodeId> InterfaceIds = [];
   
   public PooledSet<NodeId> ConstructorIds = [];
   public PooledSet<NodeId> MethodIds = [];
   public PooledSet<NodeId> PropertyIds = [];
   public PooledSet<NodeId> FieldIds = [];
   
   public PooledSet<NodeId> AttributeIds = [];
   
   public TypeComponent()
   {
      
   }
   
   public bool Equals(TypeComponent other)
   {
      return Id.Equals(other.Id);
   }

   public override bool Equals(object? obj)
   {
      return obj is TypeComponent other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Id);
   }

   public static bool operator ==(TypeComponent left, TypeComponent right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(TypeComponent left, TypeComponent right)
   {
      return !(left == right);
   }
}