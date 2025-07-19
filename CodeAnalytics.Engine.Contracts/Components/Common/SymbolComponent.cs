using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Components.Common;

[StructLayout(LayoutKind.Auto)]
public struct SymbolComponent 
   : IEquatable<SymbolComponent>
{
   public NodeId Id = NodeId.Empty;

   public StringId Name = StringId.Empty;
   public StringId MetadataName = StringId.Empty;
   public StringId FullPathName = StringId.Empty;

   public PooledSet<StringId> FileLocations = [];
   public PooledSet<StringId> Projects = [];

   public SymbolComponent()
   {
      
   }

   public bool Equals(SymbolComponent other)
   {
      return Id.Equals(other.Id);
   }

   public override bool Equals(object? obj)
   {
      return obj is SymbolComponent other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Id);
   }

   public static bool operator ==(SymbolComponent left, SymbolComponent right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(SymbolComponent left, SymbolComponent right)
   {
      return !(left == right);
   }
}