using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Engine.Common.Results.Errors;
using CodeAnalytics.Engine.Contracts.Ids.Interfaces;

namespace CodeAnalytics.Engine.Contracts.Ids;

public readonly struct StringId 
   : IEquatable<StringId>
{
   public static readonly StringId Empty = new(-1, null);
   
   public readonly int Value;
   public readonly IStringIdStore? Store;

   public StringId(int value, IStringIdStore? store)
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

      return new Error<string>($"StringId.Value ({Value}) was not found in store {Store.Name}.");
   }

   public override string ToString()
   {
      var result = GetString();
      return result is { HasValue: true, Success: { } str }
         ? str : result.Error.Detail;
   }
   
   public bool Equals(StringId other)
   {
      return Value == other.Value;
   }

   public override bool Equals(object? obj)
   {
      return obj is StringId other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Value);
   }

   public static bool operator ==(StringId left, StringId right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(StringId left, StringId right)
   {
      return !(left == right);
   }
}