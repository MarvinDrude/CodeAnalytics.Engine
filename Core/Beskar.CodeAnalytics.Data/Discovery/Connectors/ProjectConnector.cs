using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Beskar.CodeAnalytics.Data.Files;

namespace Beskar.CodeAnalytics.Data.Discovery.Connectors;

public static class ProjectConnector
{
   public static void Connect(BakeContext context)
   {
      var targetName = context.FileNames[FileIds.Project];
      var edgeName = context.FileNames[FileIds.EdgeSymbol];
      
      var targetPath = Path.Combine(context.OutputDirectoryPath, targetName);
      var edgePath = Path.Combine(context.OutputDirectoryPath, edgeName);

      using var targetHandle = new MmfHandle(targetPath, writable: true);
      using var edgeHandle = new MmfHandle(edgePath);

      using var targetBuffer = targetHandle.GetBuffer();
      using var edgeBuffer = edgeHandle.GetBuffer();

      var edgeCount = (int)(edgeHandle.Length / Unsafe.SizeOf<SymbolEdgeSpec>());
      var edges = edgeBuffer.GetSpan<SymbolEdgeSpec>(0, edgeCount);
      
      var targetCount = (int)(targetHandle.Length / Unsafe.SizeOf<ProjectSpec>());
      var targetSymbols = targetBuffer.GetSpan<ProjectSpec>(0, targetCount);

      var edgeIndex = 0;
      var targetIndex = 0;

      while (targetIndex < targetCount)
      {
         ref var targetSymbol = ref targetSymbols[targetIndex];
         
         var references = new StorageView<ProjectSpec>(-1, 0);
         var files = new StorageView<FileSpec>(-1, 0);
         var solutions = new StorageView<SolutionSpec>(-1, 0);
         
         targetSymbol.ProjectReferences = references;
         targetSymbol.Files = files;
         targetSymbol.Solutions = solutions;
         
         if (edgeIndex >= edgeCount)
         {
            targetIndex++;
            continue;
         }
         
         var currentSourceId = edges[edgeIndex].SourceSymbolId;
         if (targetSymbol.Identifier < currentSourceId)
         {
            targetIndex++;
            continue;
         }

         if (targetSymbol.Identifier > currentSourceId)
         {
            edgeIndex++;
            continue;
         }

         while (edgeIndex < edgeCount && edges[edgeIndex].SourceSymbolId == currentSourceId)
         {
            var currentType = edges[edgeIndex].Type;
            var typeStartIndex = edgeIndex;

            while (edgeIndex < edgeCount &&
                   edges[edgeIndex].SourceSymbolId == currentSourceId &&
                   edges[edgeIndex].Type == currentType)
            {
               edgeIndex++;
            }
            
            var typeCount = edgeIndex - typeStartIndex;

            switch (currentType)
            {
               case SymbolEdgeType.ProjectReference:
                  references = new StorageView<ProjectSpec>(typeStartIndex, typeCount);
                  break;
               case SymbolEdgeType.ProjectSolution:
                  solutions = new StorageView<SolutionSpec>(typeStartIndex, typeCount);
                  break;
               case SymbolEdgeType.ProjectFile:
                  files = new StorageView<FileSpec>(typeStartIndex, typeCount);
                  break;
            }
         }

         targetSymbol.ProjectReferences = references;
         targetSymbol.Files = files;
         targetSymbol.Solutions = solutions;
         
         targetIndex++;
      }
   }
}