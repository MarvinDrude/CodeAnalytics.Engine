using System.Text.Json;
using CodeAnalytics.Web.Client.Menus;
using CodeAnalytics.Web.Client.Services.Data;
using CodeAnalytics.Web.Client.Services.Source;
using CodeAnalytics.Web.Common.Services.Data;
using CodeAnalytics.Web.Common.Services.Source;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.Configure<JsonSerializerOptions>(x =>
{
   x.IncludeFields = true;
});

builder.Services.AddScoped<ISourceTextService, ClientSourceTextService>();
builder.Services.AddScoped<IExplorerService, ClientExplorerService>();
builder.Services.AddScoped<IOccurrenceService, ClientOccurrenceService>();
builder.Services.AddScoped<MenuService>();

builder.Services.AddScoped(_ => new HttpClient
{
   BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

await builder.Build().RunAsync();