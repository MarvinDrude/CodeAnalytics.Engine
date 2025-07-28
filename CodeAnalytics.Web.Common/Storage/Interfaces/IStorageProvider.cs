using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Web.Common.Storage.Models;

namespace CodeAnalytics.Web.Common.Storage.Interfaces;

public interface IStorageProvider
{
   public ValueTask<Result<bool, StorageError>> SetItemString(string key, string? value, CancellationToken ct = default);
   public ValueTask<Result<string, StorageError>> GetItemString(string key, CancellationToken ct = default);
   
   public ValueTask<Result<bool, StorageError>> Clear(CancellationToken ct = default);

   public ValueTask<Result<bool, StorageError>> DeleteItem(string key, CancellationToken ct = default)
   {
      return SetItemString(key, null, ct);
   }
}