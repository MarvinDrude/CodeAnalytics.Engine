using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Engine.Common.Results.Errors;

namespace CodeAnalytics.Engine.Contracts.Ids.Interfaces;

public interface INodeIdStore : IIdStore
{
   public static abstract INodeIdStore? Current { get; }
   
   public NodeId GetOrAdd(string value);
}