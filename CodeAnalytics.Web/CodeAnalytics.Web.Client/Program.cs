using System.Text.Json;
using CodeAnalytics.Web.Client.Menus;
using CodeAnalytics.Web.Client.Services.Data;
using CodeAnalytics.Web.Client.Services.Search;
using CodeAnalytics.Web.Client.Services.Source;
using CodeAnalytics.Web.Common.Preferences.Interfaces;
using CodeAnalytics.Web.Common.Preferences.Services;
using CodeAnalytics.Web.Common.Services.Data;
using CodeAnalytics.Web.Common.Services.Search;
using CodeAnalytics.Web.Common.Services.Source;
using CodeAnalytics.Web.Common.Storage.Interfaces;
using CodeAnalytics.Web.Common.Storage.Providers;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.Configure<JsonSerializerOptions>(x =>
{
   x.IncludeFields = true;
});

builder.Services.AddScoped<ISourceTextService, ClientSourceTextService>();
builder.Services.AddScoped<IExplorerService, ClientExplorerService>();
builder.Services.AddScoped<IOccurrenceService, ClientOccurrenceService>();
builder.Services.AddScoped<ISearchService, ClientSearchService>();
builder.Services.AddScoped<IFileSearchService, ClientFileSearchService>();
builder.Services.AddScoped<MenuService>();

builder.Services.AddKeyedScoped<IJsonStorageProvider, LocalStorageProvider>("local-storage");
builder.Services.AddKeyedScoped<IJsonStorageProvider, MemoryStorageProvider>("fallback");
builder.Services.AddSingleton<IPreferenceVersion, PreferenceVersion>();
builder.Services.AddScoped<IPreferenceService, PreferenceService>();

builder.Services.AddScoped(_ =>
{
   var client = new HttpClient
   {
      BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
   };

   return client;
});

await builder.Build().RunAsync();