using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Engine.Common.Results.Errors;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Ids.Interfaces;

namespace CodeAnalytics.Engine.Ids;

public sealed class StringIdStore : IdStoreBase, IStringIdStore
{
   private static readonly AsyncLocal<StringIdStore?> _current = new();
   public static IStringIdStore? Current
   {
      get => _current.Value;
      set => _current.Value = value as StringIdStore;
   }
   
   public StringIdStore(string name) 
      : base(name)
   {
   }

   public StringIdStore(string name, Dictionary<string, int> map, int nextId) 
      : base(name, map, nextId)
   {
   }

   public StringId GetOrAdd(string value)
   {
      return new StringId(GetOrAddId(value), this);
   }
}