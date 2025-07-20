using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Components.Inerfaces;
using CodeAnalytics.Engine.Contracts.Enums.Components;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Components.Members;

[StructLayout(LayoutKind.Auto)]
public struct ParameterComponent
   : IEquatable<ParameterComponent>, IComponent
{
   public NodeId NodeId => Id;
   public NodeId Id = NodeId.Empty;

   public NodeId TypeId = NodeId.Empty;
   public ParameterModifier Modifiers;
   
   public PooledSet<NodeId> AttributeIds = [];

   public ParameterComponent()
   {
      Modifiers = ParameterModifier.None;
   }

   public void Dispose()
   {
      AttributeIds.Dispose();
   }

   public bool Equals(ParameterComponent other)
   {
      return Id.Equals(other.Id);
   }

   public override bool Equals(object? obj)
   {
      return obj is ParameterComponent other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Id);
   }

   public static bool operator ==(ParameterComponent left, ParameterComponent right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(ParameterComponent left, ParameterComponent right)
   {
      return !(left == right);
   }
}