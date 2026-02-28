using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Beskar.CodeAnalytics.Data.Files;

namespace Beskar.CodeAnalytics.Data.Discovery.Connectors;

public static class SolutionConnector
{
   public static void Connect(BakeContext context)
   {
      var targetName = context.FileNames[FileIds.Solution];
      var edgeName = context.FileNames[FileIds.EdgeSymbol];
      
      var targetPath = Path.Combine(context.OutputDirectoryPath, targetName);
      var edgePath = Path.Combine(context.OutputDirectoryPath, edgeName);

      using var targetHandle = new MmfHandle(targetPath, writable: true);
      using var edgeHandle = new MmfHandle(edgePath);

      using var targetBuffer = targetHandle.GetBuffer();
      using var edgeBuffer = edgeHandle.GetBuffer();

      var edgeCount = (int)(edgeHandle.Length / Unsafe.SizeOf<SymbolEdgeSpec>());
      var edges = edgeBuffer.GetSpan<SymbolEdgeSpec>(0, edgeCount);
      
      var targetCount = (int)(targetHandle.Length / Unsafe.SizeOf<SolutionSpec>());
      var targetSymbols = targetBuffer.GetSpan<SolutionSpec>(0, targetCount);

      var edgeIndex = 0;
      var targetIndex = 0;

      while (targetIndex < targetCount)
      {
         ref var targetSymbol = ref targetSymbols[targetIndex];
         
         var projects = new StorageView<ProjectSpec>(-1, 0);
         
         targetSymbol.Projects = projects;
         
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
               case SymbolEdgeType.SolutionProject:
                  projects = new StorageView<ProjectSpec>(typeStartIndex, typeCount);
                  break;
            }
         }

         targetSymbol.Projects = projects;
         
         targetIndex++;
      }
   }
}