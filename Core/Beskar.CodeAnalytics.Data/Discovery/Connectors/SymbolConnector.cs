using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Files;

namespace Beskar.CodeAnalytics.Data.Discovery.Connectors;

public static class SymbolConnector
{
   public static void Connect(BakeContext context)
   {
      var targetName = context.FileNames[FileIds.Symbol];
      var locationName = context.FileNames[FileIds.FileLocation];
      
      var targetPath = Path.Combine(context.OutputDirectoryPath, targetName);
      var locationPath = Path.Combine(context.OutputDirectoryPath, locationName);

      using var targetHandle = new MmfHandle(targetPath, writable: true);
      using var locationHandle = new MmfHandle(locationPath);

      using var targetBuffer = targetHandle.GetBuffer();
      using var locationBuffer = locationHandle.GetBuffer();

      var locationCount = (int)(locationHandle.Length / Unsafe.SizeOf<SymbolLocationSpec>());
      var locations = locationBuffer.GetSpan<SymbolLocationSpec>(0, locationCount);
      
      var targetCount = (int)(targetHandle.Length / Unsafe.SizeOf<SymbolSpec>());
      var targetSymbols = targetBuffer.GetSpan<SymbolSpec>(0, targetCount);

      var locIndex = 0;
      var targetIndex = 0;

      while (targetIndex < targetCount)
      {
         ref var targetSymbol = ref targetSymbols[targetIndex];
         
         var declarations = new StorageView<SymbolLocationSpec>(-1, 0);
         var locationsView = new StorageView<SymbolLocationSpec>(-1, 0);
         
         if (locIndex >= locationCount)
         {
            targetSymbol.Declarations = declarations;
            targetSymbol.Locations = locationsView;
            targetIndex++;
            continue;
         }
         
         var currentSymbolId = locations[locIndex].SymbolId;

         if (targetSymbol.Id < currentSymbolId)
         {
            targetSymbol.Declarations = declarations;
            targetSymbol.Locations = locationsView;
            targetIndex++;
            continue;
         }

         if (targetSymbol.Id > currentSymbolId)
         {
            locIndex++;
            continue;
         }

         while (locIndex < locationCount && locations[locIndex].SymbolId == currentSymbolId)
         {
            var isDecl = locations[locIndex].IsDeclaration;
            var startIndex = locIndex;

            while (locIndex < locationCount &&
                   locations[locIndex].SymbolId == currentSymbolId &&
                   locations[locIndex].IsDeclaration == isDecl)
            {
               locIndex++;
            }
            
            var groupCount = locIndex - startIndex;

            if (isDecl)
            {
               declarations = new StorageView<SymbolLocationSpec>(startIndex, groupCount);
            }
            else
            {
               locationsView = new StorageView<SymbolLocationSpec>(startIndex, groupCount);
            }
         }

         targetSymbol.Declarations = declarations;
         targetSymbol.Locations = locationsView;
         
         targetIndex++;
      }
   }
}