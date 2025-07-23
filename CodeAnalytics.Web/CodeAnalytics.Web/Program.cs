using System.Text.Json;
using CodeAnalytics.Web.Client.Menus;
using CodeAnalytics.Web.Common.Services.Data;
using CodeAnalytics.Web.Common.Services.Source;
using CodeAnalytics.Web.Components;
using CodeAnalytics.Web.Endpoints;
using CodeAnalytics.Web.Options;
using CodeAnalytics.Web.Services.Data;
using CodeAnalytics.Web.Services.Source;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
   .AddInteractiveServerComponents()
   .AddInteractiveWebAssemblyComponents();

builder.Services.Configure<JsonOptions>(x =>
{
   x.SerializerOptions.IncludeFields = true;
});

builder.Services.Configure<CodeOptions>(builder.Configuration.GetSection("Code"));

builder.Services.AddSingleton<ISourceTextService, ServerSourceTextService>();
builder.Services.AddSingleton<IExplorerService, ServerExplorerService>();
builder.Services.AddSingleton<IDataService, ServerDataService>();
builder.Services.AddSingleton<IOccurrenceService, ServerOccurrenceService>();

builder.Services.AddScoped<MenuService>();

builder.Services.AddResponseCompression();

var app = builder.Build();

var dataService = app.Services.GetRequiredService<IDataService>();
await dataService.GetAnalyzeStore();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseWebAssemblyDebugging();
}
else
{
   app.UseExceptionHandler("/Error", createScopeForErrors: true);
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
}

app.UseResponseCompression();

app.UseHttpsRedirection();
app.UseAntiforgery();

EndpointMapper.Map(app);

app.MapStaticAssets();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode()
   .AddInteractiveWebAssemblyRenderMode()
   .AddAdditionalAssemblies(typeof(CodeAnalytics.Web.Client._Imports).Assembly);

app.Run();