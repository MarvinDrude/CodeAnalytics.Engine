using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Ids.Interfaces;
using CodeAnalytics.Engine.Extensions.Symbols;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Ids;

public sealed class NodeIdStore : IdStoreBase, INodeIdStore
{
   private static readonly AsyncLocal<NodeIdStore?> _current = new();
   public static INodeIdStore? Current
   {
      get => _current.Value;
      set => _current.Value = value as NodeIdStore;
   }
   
   public NodeIdStore(string name) 
      : base(name)
   {
   }

   public NodeIdStore(string name, Dictionary<string, int> map, int nextId) 
      : base(name, map, nextId)
   {
   }

   public NodeId GetOrAdd(ISymbol symbol)
   {
      return GetOrAdd(symbol.GenerateId());
   }

   public NodeId GetOrAdd(string value)
   {
      return new NodeId(GetOrAddId(value), this);
   }
}