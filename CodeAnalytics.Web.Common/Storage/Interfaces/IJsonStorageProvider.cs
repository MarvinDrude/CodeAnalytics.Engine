using System.Text.Json;
using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Web.Common.Storage.Models;

namespace CodeAnalytics.Web.Common.Storage.Interfaces;

public interface IJsonStorageProvider : IStorageProvider
{
   public async ValueTask<Result<TValue, StorageError>> GetItem<TValue>(string key, CancellationToken ct = default)
   {
      var rawResult = await GetItemString(key, ct);
      if (rawResult is { HasValue: false })
      {
         return rawResult.Error;
      }
      
      try
      {
         var json = JsonSerializer.Deserialize<TValue>(rawResult.Success);
         if (json is null)
         {
            return new StorageError(StorageErrorType.KeyNotFound, null);
         }

         return json;
      }
      catch (Exception err)
      {
         return new StorageError(StorageErrorType.JsonError, err.ToString());
      }
   }

   public async ValueTask<Result<bool, StorageError>> SetItem<TValue>(
      string key, TValue value, CancellationToken ct = default)
   {
      try
      {
         var rawString = JsonSerializer.Serialize(value);
         return await SetItemString(key, rawString, ct);
      }
      catch (Exception err)
      {
         return new StorageError(StorageErrorType.JsonError, err.ToString());
      }
   }
}