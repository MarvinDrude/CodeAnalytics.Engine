using CodeAnalytics.Web.Common.Preferences.Interfaces;
using CodeAnalytics.Web.Common.Preferences.Models;
using CodeAnalytics.Web.Common.Storage.Interfaces;
using CodeAnalytics.Web.Common.Storage.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CodeAnalytics.Web.Common.Preferences.Services;

public sealed class PreferenceService : IPreferenceService
{
   private readonly IJsonStorageProvider _storageProvider;
   private readonly IJsonStorageProvider _fallbackStorageProvider;
   private readonly IPreferenceVersion _versions;

   public PreferenceService(
      [FromKeyedServices("local-storage")] IJsonStorageProvider storageProvider,
      [FromKeyedServices("fallback")] IJsonStorageProvider fallbackProvider,
      IPreferenceVersion versions)
   {
      _storageProvider = storageProvider;
      _versions = versions;
      _fallbackStorageProvider = fallbackProvider;
   }

   public int GetLatestVersion<TPreference>()
      where TPreference : IPreference
   {
      return _versions.GetLatestVersion<TPreference>();
   }

   public async Task Update<TPreference>(TPreference preference)
      where TPreference : IPreference
   {
      var keyName = typeof(TPreference).Name;
      var result = await _storageProvider.SetItem(keyName, preference);

      switch (result)
      {
         case { HasValue: true }:
            return;
         case { Error.Type: StorageErrorType.StorageError }:
            await _fallbackStorageProvider.SetItem(keyName, preference);
            break;
      }
   }
   
   public async Task<TPreference> GetOrCreate<TPreference>(Func<Task<TPreference>> createFactory)
      where TPreference : IPreference
   {
      var keyName = typeof(TPreference).Name;

      var versionNumber = _versions.GetLatestVersion<TPreference>();
      var preferenceResult = await _storageProvider.GetItem<TPreference>(keyName);

      if (preferenceResult is { HasValue: true, Success: { } preference }
          && preference.Version >= versionNumber)
      {
         return preference;
      }

      if (preferenceResult is { Error.Type: StorageErrorType.StorageError })
      {
         var fallbackResult = await _fallbackStorageProvider.GetItem<TPreference>(keyName);

         if (fallbackResult is { HasValue: true, Success: { } fallback })
         {
            return fallback;
         }
         
         return await Create(_fallbackStorageProvider, createFactory);
      }

      return await Create(_storageProvider, createFactory);
   }

   private async Task<TPreference> Create<TPreference>(
      IJsonStorageProvider provider, Func<Task<TPreference>> createFactory)
      where TPreference : IPreference
   {
      var keyName = typeof(TPreference).Name;
      var preference = await createFactory();

      await provider.SetItem(keyName, preference);
      return preference;
   }
}