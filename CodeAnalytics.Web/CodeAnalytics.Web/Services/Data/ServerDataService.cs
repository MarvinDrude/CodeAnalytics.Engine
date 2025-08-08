using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Analyze;
using CodeAnalytics.Engine.Analyze.Interfaces;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Compression;
using CodeAnalytics.Engine.Serialization;
using CodeAnalytics.Engine.Serialization.Stores;
using CodeAnalytics.Web.Common.Services.Data;
using CodeAnalytics.Web.Options;
using Microsoft.Extensions.Options;

namespace CodeAnalytics.Web.Services.Data;

public sealed class ServerDataService : IDataService
{
   private readonly IOptionsMonitor<CodeOptions> _optionsMonitor;
   private CodeOptions Options => _optionsMonitor.CurrentValue;

   [MemberNotNullWhen(true, nameof(_analyzeStore))]
   private bool IsInitialized { get; set; } = false;
   private AnalyzeStore? _analyzeStore;
   
   public ServerDataService(IOptionsMonitor<CodeOptions> optionsMonitor)
   {
      _optionsMonitor = optionsMonitor;
      _optionsMonitor.OnChange(_ => IsInitialized = false);
   }
   
   public async Task<AnalyzeStore> GetAnalyzeStore()
   {
      await EnsureInitialized();
      return _analyzeStore ?? throw new InvalidOperationException("Analyze store is not initialized");
   }
   
   public ValueTask<AnalyzeStore> GetStore()
   {
      return new ValueTask<AnalyzeStore>(GetAnalyzeStore());
   }
   
   private ValueTask EnsureInitialized()
   {
      return IsInitialized ? 
         ValueTask.CompletedTask
         : new ValueTask(Reload());
   }
   
   private async Task Reload()
   {
      IsInitialized = false;
      
      _analyzeStore?.Inner.Dispose();
      _analyzeStore?.Dispose();

      var filePath = Path.Combine(Options.DataFolderPath, "data.caec");
      var bytes = await File.ReadAllBytesAsync(filePath);

      _analyzeStore = new AnalyzeStore(
         Serializer<CollectorStore, CollectorStoreSerializer>.FromMemory(
            new DeflateCompressor().Decompress(bytes).Span));
      
      IsInitialized = true;
   }
}