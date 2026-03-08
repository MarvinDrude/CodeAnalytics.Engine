using Beskar.CodeAnalytics.Dashboard.Components;
using Beskar.CodeAnalytics.Dashboard.Services.Database;
using Beskar.CodeAnalytics.Dashboard.Services.Structure;
using Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Services;
using Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Structure;
using Beskar.CodeAnalytics.Dashboard.Shared.Models.Structure;
using Beskar.CodeAnalytics.Dashboard.Shared.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
   .AddInteractiveServerComponents();

builder.Services.Configure<DashboardOptions>(
   builder.Configuration.GetSection("Dashboard"));

builder.Services.AddSingleton<IDatabaseProvider, DatabaseProvider>()
   .AddSingleton<IDatabaseScheduler, DatabaseScheduler>()
   .AddSingleton<IFolderService, ImFolderService>();

var app = builder.Build();

var dbProvider = app.Services.GetRequiredService<IDatabaseProvider>();
if (dbProvider is DatabaseProvider provider)
{
   await provider.Initialize();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
   app.UseExceptionHandler("/Error", createScopeForErrors: true);
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();