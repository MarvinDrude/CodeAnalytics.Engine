using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Collector.Extensions;
using CodeAnalytics.Engine.Contracts.Components.Common;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Components.Common;

public sealed class SymbolCollector 
   : IComponentCollector<SymbolComponent, ISymbol>
{
   public static bool TryParse(
      ISymbol symbol, CollectContext context, out SymbolComponent component)
   {
      var store = context.Store;
      component = new SymbolComponent()
      {
         Id = store.NodeIdStore.GetOrAdd(symbol),
         Name = store.StringIdStore.GetOrAdd(symbol.Name),
         MetadataName = store.StringIdStore.GetOrAdd(symbol.MetadataName),
         FullPathName = store.StringIdStore.GetOrAdd(symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
      };

      var project = store.StringIdStore.GetOrAdd(context.Options.RelativePath);
      component.Projects.Add(project);
      
      foreach (var reference in symbol.DeclaringSyntaxReferences)
      {
         var path = context.GetRelativePath(reference.SyntaxTree.FilePath);
         var pathId = store.StringIdStore.GetOrAdd(path);

         component.FileLocations.Add(pathId);

         var syntax = reference.GetSyntax();
         if (syntax.SyntaxTree.FilePath != context.SyntaxTree.FilePath) continue;

         store.LineCountStore.CalculateLineStats(
            component.Id, syntax, context);
      }
      
      return true;
   }
}