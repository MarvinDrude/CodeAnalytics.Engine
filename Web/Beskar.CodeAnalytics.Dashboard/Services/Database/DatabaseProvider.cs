using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Services;
using Beskar.CodeAnalytics.Dashboard.Shared.Options;
using Beskar.CodeAnalytics.Data.Metadata.Models;
using Microsoft.Extensions.Options;

namespace Beskar.CodeAnalytics.Dashboard.Services.Database;

public sealed class DatabaseProvider(
   IOptionsMonitor<DashboardOptions> optionsMonitor) : IDatabaseProvider, IDisposable
{
   private readonly IOptionsMonitor<DashboardOptions> _optionsMonitor = optionsMonitor;
   private DashboardOptions Options => _optionsMonitor.CurrentValue;

   [MemberNotNullWhen(true, nameof(_descriptor))]
   private bool IsInitialized => _descriptor != null;   
   private DatabaseDescriptor? _descriptor;
   
   public async Task Initialize()
   {
      _descriptor = await DatabaseDescriptor.Create(Options.DatabaseFolderPath);
   }
   
   public DatabaseDescriptor GetDescriptor()
   {
      return !IsInitialized 
         ? throw new InvalidOperationException("Database is not initialized.") 
         : _descriptor;
   }

   public void Dispose()
   {
      if (!IsInitialized) return;
      
      _descriptor.Dispose();
      _descriptor = null;
   }
}