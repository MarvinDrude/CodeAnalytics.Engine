using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Beskar.CodeAnalytics.Data.Extensions;
using Beskar.CodeAnalytics.Data.Files;

namespace Beskar.CodeAnalytics.Data.Discovery.Connectors;

public static class NamedTypeSymbolConnector
{
   public static void Connect(BakeContext context)
   {
      var targetName = context.FileNames[FileIds.NamedTypeSymbol];
      var edgeName = context.FileNames[FileIds.EdgeSymbol];
      
      var targetPath = Path.Combine(context.OutputDirectoryPath, targetName);
      var edgePath = Path.Combine(context.OutputDirectoryPath, edgeName);

      using var targetHandle = new MmfHandle(targetPath, writable: true);
      using var edgeHandle = new MmfHandle(edgePath);

      using var targetBuffer = targetHandle.GetBuffer();
      using var edgeBuffer = edgeHandle.GetBuffer();

      var edgeCount = (int)(edgeHandle.Length / Unsafe.SizeOf<SymbolEdgeSpec>());
      var edges = edgeBuffer.GetSpan<SymbolEdgeSpec>(0, edgeCount);
      
      var targetCount = (int)(targetHandle.Length / Unsafe.SizeOf<NamedTypeSymbolSpec>());
      var targetSymbols = targetBuffer.GetSpan<NamedTypeSymbolSpec>(0, targetCount);

      var currentEdgeIndex = 0;
      while (currentEdgeIndex < edgeCount)
      {
         var currentEdge = edges[currentEdgeIndex];
         var currentSourceId = currentEdge.SourceSymbolId;

         var typeParameters = new StorageView<TypeParameterSymbolSpec>(-1, 0);
         
         var methods = new StorageView<MethodSymbolSpec>(-1, 0);
         var instanceConstructors = new StorageView<MethodSymbolSpec>(-1, 0);
         var staticConstructors = new StorageView<MethodSymbolSpec>(-1, 0);

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
               case SymbolEdgeType.TypeParameter:
                  typeParameters = new StorageView<TypeParameterSymbolSpec>(typeStartIndex, typeCount);
                  break;
               case SymbolEdgeType.Method:
                  methods = new StorageView<MethodSymbolSpec>(typeStartIndex, typeCount);
                  break;
               case SymbolEdgeType.InstanceConstructor:
                  instanceConstructors = new StorageView<MethodSymbolSpec>(typeStartIndex, typeCount);
                  break;
               case SymbolEdgeType.StaticConstructor:
                  staticConstructors = new StorageView<MethodSymbolSpec>(typeStartIndex, typeCount);
                  break;
            }
         }

         var targetIndex = targetSymbols.BinaryFindIndex(currentSourceId);
         if (targetIndex != -1)
         {
            ref var symbol = ref targetSymbols[targetIndex];
            
            symbol.TypeParameters = typeParameters;
            
            symbol.Methods = methods;
            symbol.InstanceConstructors = instanceConstructors;
            symbol.StaticConstructors = staticConstructors;
         }
      }
   }
}