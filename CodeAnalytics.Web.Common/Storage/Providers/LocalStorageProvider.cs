using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Web.Common.Storage.Interfaces;
using CodeAnalytics.Web.Common.Storage.Models;
using Microsoft.JSInterop;

namespace CodeAnalytics.Web.Common.Storage.Providers;

public sealed class LocalStorageProvider : IJsonStorageProvider
{
   private readonly IJSRuntime _jsRuntime;

   public LocalStorageProvider(IJSRuntime jsRuntime)
   {
      _jsRuntime = jsRuntime;
   }
   
   public async ValueTask<Result<bool, StorageError>> SetItemString(
      string key, string? value, CancellationToken ct = default)
   {
      try
      {
         if (value is null)
         {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", ct, key, value);
         }
         else
         {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", ct, key, value);
         }
         
         return true;
      }
      catch (Exception ex)
      {
         return new StorageError(StorageErrorType.StorageError, ex.ToString());
      }
   }

   public async ValueTask<Result<string, StorageError>> GetItemString(
      string key, CancellationToken ct = default)
   {
      try
      {
         var raw = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", ct, key);
         if (raw is null)
         {
            return new StorageError(StorageErrorType.KeyNotFound, null);
         }

         return raw;
      }
      catch (Exception ex)
      {
         return new StorageError(StorageErrorType.StorageError, ex.ToString());
      }
   }

   public async ValueTask<Result<bool, StorageError>> Clear(CancellationToken ct = default)
   {
      try
      {
         await _jsRuntime.InvokeVoidAsync("localStorage.clear", ct);
         return true;
      }
      catch (Exception ex)
      {
         return new StorageError(StorageErrorType.StorageError, ex.ToString());
      }
   }
}