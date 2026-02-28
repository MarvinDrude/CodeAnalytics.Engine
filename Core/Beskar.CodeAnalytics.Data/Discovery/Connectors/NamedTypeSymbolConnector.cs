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

      var edgeIndex = 0;
      var targetIndex = 0;

      while (targetIndex < targetCount)
      {
         ref var targetSymbol = ref targetSymbols[targetIndex];
         
         var typeParameters = new StorageView<TypeParameterSymbolSpec>(-1, 0);
         var methods = new StorageView<MethodSymbolSpec>(-1, 0);
         var instanceConstructors = new StorageView<MethodSymbolSpec>(-1, 0);
         var staticConstructors = new StorageView<MethodSymbolSpec>(-1, 0);
         
         targetSymbol.TypeParameters = typeParameters;
         targetSymbol.Methods = methods;
         targetSymbol.InstanceConstructors = instanceConstructors;
         targetSymbol.StaticConstructors = staticConstructors;
         
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

         targetSymbol.TypeParameters = typeParameters;
         targetSymbol.Methods = methods;
         targetSymbol.InstanceConstructors = instanceConstructors;
         targetSymbol.StaticConstructors = staticConstructors;
         
         targetIndex++;
      }
   }
}