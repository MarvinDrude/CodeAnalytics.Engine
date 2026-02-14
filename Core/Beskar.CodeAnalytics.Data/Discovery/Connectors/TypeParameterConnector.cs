using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Beskar.CodeAnalytics.Data.Extensions;
using Beskar.CodeAnalytics.Data.Files;

namespace Beskar.CodeAnalytics.Data.Discovery.Connectors;

public static class TypeParameterConnector
{
   public static void Connect(BakeContext context)
   {
      var targetName = context.FileNames[FileIds.TypeParameterSymbol];
      var edgeName = context.FileNames[FileIds.EdgeSymbol];
      
      var targetPath = Path.Combine(context.OutputDirectoryPath, targetName);
      var edgePath = Path.Combine(context.OutputDirectoryPath, edgeName);

      using var targetHandle = new MmfHandle(targetPath, writable: true);
      using var edgeHandle = new MmfHandle(edgePath);

      using var targetBuffer = targetHandle.GetBuffer();
      using var edgeBuffer = edgeHandle.GetBuffer();

      var edgeCount = (int)(edgeHandle.Length / Unsafe.SizeOf<SymbolEdgeSpec>());
      var edges = edgeBuffer.GetSpan<SymbolEdgeSpec>(0, edgeCount);
      
      var targetCount = (int)(targetHandle.Length / Unsafe.SizeOf<TypeParameterSymbolSpec>());
      var targetSymbols = targetBuffer.GetSpan<TypeParameterSymbolSpec>(0, targetCount);

      var currentEdgeIndex = 0;
      while (currentEdgeIndex < edgeCount)
      {
         var currentEdge = edges[currentEdgeIndex];
         var currentSourceId = currentEdge.SourceSymbolId;

         var constraints = new StorageView<TypeSymbolSpec>(-1, 0);

         while (currentEdgeIndex < edgeCount 
                && currentEdge.SourceSymbolId == currentSourceId)
         {
            var currentType = currentEdge.Type;
            var typeStartIndex = currentEdgeIndex;

            while (currentEdgeIndex < edgeCount &&
                   currentEdge.SourceSymbolId == currentSourceId &&
                   currentEdge.Type == currentType)
            {
               currentEdgeIndex++;
               
               if (currentEdgeIndex < edgeCount) 
                  currentEdge = edges[currentEdgeIndex];
            }
            
            var typeCount = currentEdgeIndex - typeStartIndex;

            switch (currentType)
            {
               case SymbolEdgeType.ConstraintType:
                  constraints = new StorageView<TypeSymbolSpec>(typeStartIndex, typeCount);
                  break;
            }
         }

         var targetIndex = targetSymbols.BinaryFindIndex(currentSourceId);
         if (targetIndex != -1)
         {
            ref var symbol = ref targetSymbols[targetIndex];
            
            symbol.ConstraintTypes = constraints;
         }
      }
   }
}