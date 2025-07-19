using System.Diagnostics.CodeAnalysis;

namespace CodeAnalytics.Engine.Contracts.Ids.Interfaces;

public interface IStringIdStore : IIdStore
{
   public static abstract IStringIdStore? Current { get; }
   
   public StringId GetOrAdd(string value);
}