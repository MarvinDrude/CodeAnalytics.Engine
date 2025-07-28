using System.Collections.Concurrent;
using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Web.Common.Storage.Interfaces;
using CodeAnalytics.Web.Common.Storage.Models;

namespace CodeAnalytics.Web.Common.Storage.Providers;

public sealed class MemoryStorageProvider : IJsonStorageProvider
{
   private readonly ConcurrentDictionary<string, string> _storage = [];
   
   public ValueTask<Result<bool, StorageError>> SetItemString(string key, string? value, CancellationToken ct = default)
   {
      if (value is null)
      {
         _storage.TryRemove(key, out _);
      }
      else
      {
         _storage[key] = value;
      }
      
      return new ValueTask<Result<bool, StorageError>>(true);
   }

   public ValueTask<Result<string, StorageError>> GetItemString(string key, CancellationToken ct = default)
   {
      return _storage.TryGetValue(key, out var value) 
         ? new ValueTask<Result<string, StorageError>>(value) 
         : new ValueTask<Result<string, StorageError>>(new StorageError(StorageErrorType.KeyNotFound, null));
   }

   public ValueTask<Result<bool, StorageError>> Clear(CancellationToken ct = default)
   {
      _storage.Clear();
      return ValueTask.FromResult(new Result<bool, StorageError>(true));
   }
}