using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Engine.Common.Results.Errors;
using CodeAnalytics.Engine.Contracts.Ids.Interfaces;

namespace CodeAnalytics.Engine.Contracts.Ids;

public readonly struct NodeId 
   : IEquatable<NodeId>
{
   public static readonly NodeId Empty = new NodeId(-1, null);

   public bool IsEmpty => Empty == this;
   
   public readonly int Value;
   public readonly INodeIdStore? Store;

   public NodeId(int value, INodeIdStore? store)
   {
      Value = value;
      Store = store;
   }
   
   public Result<string, Error<string>> GetString()
   {
      if (Store is null)
      {
         return Value == -1 ? "EMPTY" : Value.ToString();
      }

      if (Store.GetById(Value) is { } str)
      {
         return str;
      }

      return new Error<string>($"NodeId.Value ({Value}) was not found in store {Store.Name}.");
   }

   public override string ToString()
   {
      var result = GetString();
      return result is { HasValue: true, Success: { } str }
         ? str : result.Error.Detail;
   }

   public bool Equals(NodeId other)
   {
      return Value == other.Value;
   }

   public override bool Equals(object? obj)
   {
      return obj is NodeId other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Value);
   }

   public static bool operator ==(NodeId left, NodeId right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(NodeId left, NodeId right)
   {
      return !(left == right);
   }
   
   public static implicit operator int(NodeId nodeId) => nodeId.Value;
}