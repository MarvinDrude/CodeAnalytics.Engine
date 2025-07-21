using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Engine.Common.Results.Errors;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.TextRendering;
using CodeAnalytics.Engine.Serialization.Collections;
using CodeAnalytics.Engine.Serialization.TextRendering;
using CodeAnalytics.Web.Common.Services.Data;
using CodeAnalytics.Web.Common.Services.Source;
using CodeAnalytics.Web.Options;
using Microsoft.Extensions.Options;

namespace CodeAnalytics.Web.Services.Source;

public sealed class ServerSourceTextService : ISourceTextService
{
   private readonly IDataService _dataService;
   
   private readonly IOptionsMonitor<CodeOptions> _optionsMonitor;
   private CodeOptions Options => _optionsMonitor.CurrentValue;
   
   public ServerSourceTextService(
      IOptionsMonitor<CodeOptions> optionsMonitor,
      IDataService dataService)
   {
      _optionsMonitor = optionsMonitor;
      _dataService = dataService;
   }
   
   public Task<Result<SyntaxSpan[], Error<string>>> GetSyntaxSpansByStringId(StringId stringId)
   {
      return GetSyntaxSpansByPath(stringId.ToString());
   }

   public async Task<Result<SyntaxSpan[], Error<string>>> GetSyntaxSpansByPath(string path)
   {
      path = Path.ChangeExtension(path, "csspan");
      var completePath = Path.Combine(Options.DataFolderPath, path);

      if (!File.Exists(completePath))
      {
         return new Error<string>("File not found.");
      }

      try
      {
         var bytes = await File.ReadAllBytesAsync(completePath);
         var reader = new ByteReader(bytes);

         if (!PooledListSerializer<SyntaxSpan, SyntaxSpanSerializer>
                .TryDeserialize(ref reader, out var spans))
         {
            return new Error<string>("Could not deserialize syntax spans.");
         }

         var store = await _dataService.GetAnalyzeStore();
         
         using var dispose = spans;
         foreach (ref var span in spans)
         {
            if (!span.Reference.IsEmpty
                && !store.ContainingDeclarationIds.Contains(span.Reference))
            {
               span.Reference = NodeId.Empty;
            }
         }
         
         return dispose.WrittenSpan.ToArray();
      }
      catch
      {
         return new Error<string>("Error at reading file.");
      }
   }
}