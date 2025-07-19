using System.Diagnostics.CodeAnalysis;

namespace CodeAnalytics.Engine.Contracts.Ids.Interfaces;

public interface IIdStore
{
   public int NextId { get; }
   public string Name { get; }
   
   public string? GetById(int id);
   public bool TryGetById(int id, [MaybeNullWhen(false)] out string value);

   public int GetOrAddId(string value);

   public Dictionary<string, int> ToDictionary();
}