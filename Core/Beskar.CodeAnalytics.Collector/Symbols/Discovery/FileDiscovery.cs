using System.Collections.Concurrent;
using System.Text;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Collector.Source;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Structure;
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

      var projectFilePath = Path.GetDirectoryName(context.ProjectHandle.Project.FilePath ?? "") ?? "";
      var projectRelativePath = Path.GetRelativePath(context.DiscoveryBatch.Options.BasePath, projectFilePath ?? "");

      var folderRelative = Path.GetDirectoryName(filePath);
      var isInRoot = folderRelative is null or ".";
      folderRelative = isInRoot ? "" : folderRelative;

      var folderId = batch.FolderTreeBuilder.GetOrCreateFolder(batch, folderRelative ?? "");
      var id = batch.Identifiers.GenerateIdentifier(filePath, filePathDef);

      if (!_alreadyInProjectDiscovered.TryAdd((context.ProjectId, folderId, id), true))
      {
         return false;
      }
      
      batch.WriteDiscoveryEdge(context.ProjectId, id, SymbolEdgeType.ProjectFile);

      if (!_alreadyDiscovered.TryAdd((folderId, id), true))
      {
         return false;
      }
      
      batch.WriteDiscoveryEdge(folderId, id, SymbolEdgeType.FolderToFile);
      
      var fileName = Path.GetFileName(filePath);
      var fileNameDef = batch.StringDefinitions.GetStringFileView(fileName);
      
      var spec = new FileSpec()
      {
         Id = id,
         ParentId = folderId,
         
         FullPath = filePathDef,
         Name = fileNameDef,
         
         Kind = FileKind.CSharp,
         
         Declarations = new StorageView<SymbolSpec>(-1, -1),
         Symbols = new StorageView<SymbolSpec>(-1, -1),
      };

      await batch.FileWriter.Write(id, spec);

      var syntaxFile = await new SourceTokenizer(id, context, spans)
         .Tokenize(CancellationToken.None);
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
   
   /// <summary>
   /// Small workaround to avoid duplicate files in case they are referenced in multiple projects / solutions.
   /// </summary>
   private static readonly ConcurrentDictionary<(uint ProjectId, uint FolderId, uint FileId), bool> _alreadyInProjectDiscovered = [];
   private static readonly ConcurrentDictionary<(uint FolderId, uint FileId), bool> _alreadyDiscovered = [];
}