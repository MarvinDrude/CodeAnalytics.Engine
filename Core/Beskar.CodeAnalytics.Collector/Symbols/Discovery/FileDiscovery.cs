using System.Text;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Collector.Source;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Microsoft.CodeAnalysis.Text;

namespace Beskar.CodeAnalytics.Collector.Symbols.Discovery;

public static class FileDiscovery
{
   public static async Task<bool> Discover(DiscoverContext context, Dictionary<TextSpan, TextSpanCacheEntry> spans)
   {
      var batch = context.DiscoveryBatch;
      
      var filePath = GetFilePath(context);
      var filePathDef = batch.StringDefinitions.GetStringFileView(filePath);

      var id = batch.Identifiers.GenerateIdentifier(filePath, filePathDef);
      batch.WriteDiscoveryEdge(context.ProjectId, id, SymbolEdgeType.ProjectFile);
      
      var spec = new FileSpec()
      {
         Id = id,
         FullPath = filePathDef,
         
         Declarations = new StorageView<SymbolSpec>(-1, -1),
         Symbols = new StorageView<SymbolSpec>(-1, -1),
      };

      await batch.FileWriter.Write(id, spec);

      var syntaxFile = await new SourceTokenizer(context, spans)
         .Tokenize(id, CancellationToken.None);
      batch.SyntaxDiscoveryFileWriter.Write(syntaxFile);
      
      return true;
   }

   private static string GetFilePath(DiscoverContext context)
   {
      if (context.SyntaxTree.FilePath is { Length: > 0 } filePath)
      {
         return Path.GetRelativePath(context.DiscoveryBatch.Options.BasePath, filePath);
      }

      var bytes = context.SourceText.GetChecksum().AsSpan();
      return Encoding.UTF8.GetString(bytes).Replace("-", "");
   }
}