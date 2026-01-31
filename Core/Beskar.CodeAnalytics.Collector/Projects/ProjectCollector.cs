using Beskar.CodeAnalytics.Collector.Extensions;
using Beskar.CodeAnalytics.Collector.Identifiers;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Storage.Entities.Misc;
using Beskar.CodeAnalytics.Storage.Entities.Symbols;
using Beskar.CodeAnalytics.Storage.Hashing;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Projects;

public sealed class ProjectCollector(CsProjectHandle handle)
{
   private readonly CsProjectHandle _handle = handle;

   public async Task Discover(DiscoveryBatch batch, CancellationToken ct = default)
   {
      var compilation = await _handle.GetCompilation(ct);

      foreach (var tree in compilation.SyntaxTrees)
      {
         var semanticModel = compilation.GetSemanticModel(tree, ignoreAccessibility: true);
         var root = await tree.GetRootAsync(ct);
         
         var document = _handle.SolutionHandle.Solution.GetDocument(tree);
         var context = new DiscoverContext()
         {
            ProjectHandle = _handle,
            SemanticModel = semanticModel,
            SourceText = await tree.GetTextAsync(ct),
            SyntaxNode = root,
            SyntaxTree = tree,
            CancellationToken = ct,
            Document = document,
            DiscoveryBatch = batch
         };

         foreach (var node in root.DescendantNodesAndSelf(descendIntoTrivia: false))
         {
            context.SyntaxNode = node;
            context.ResetSymbol();

            await HandleNode(context);
         }
      }
   }

   private async Task HandleNode(DiscoverContext context)
   {
      if (context.Symbol is not { } symbol
          || UniqueIdentifier.Create(symbol) is not { } uniqueIdentifier)
      {
         return;
      }

      var batch = context.DiscoveryBatch;
      
      var stringDefinition = batch.StringDefinitions.GetStringDefinition(uniqueIdentifier);
      var deterministicId = batch.Identifiers.GetDeterministicId(uniqueIdentifier, stringDefinition);

      ulong containingId = 0;
      var hasContaining = false;
      
      if (UniqueIdentifier.Create(symbol.ContainingSymbol) is { } containingIdentifier)
      {
         var cStringDefinition = batch.StringDefinitions.GetStringDefinition(containingIdentifier);
         var cDeterministicId = batch.Identifiers.GetDeterministicId(containingIdentifier, cStringDefinition);
         
         hasContaining = true;
         containingId = cDeterministicId;
      }
      
      var nameDef = batch.StringDefinitions.GetStringDefinition(context.Symbol.Name);
      var metadataDef = batch.StringDefinitions.GetStringDefinition(context.Symbol.MetadataName);
      var fullPathDef = batch.StringDefinitions.GetStringDefinition(context.Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
      
      var symbolDefinition = new SymbolDefinition()
      {
         Id = deterministicId,
         ContainingId = containingId,
         HasContaining = hasContaining,
         
         Name = nameDef,
         MetadataName = metadataDef,
         FullPathName = fullPathDef,
         
         IsAbstract = symbol.IsAbstract,
         IsSealed = symbol.IsSealed,
         IsStatic = symbol.IsStatic,
         IsVirtual = symbol.IsVirtual,
         IsOverride = symbol.IsOverride,
         IsExtern = symbol.IsExtern,
         IsImplicitlyDeclared = symbol.IsImplicitlyDeclared,
         
         Accessibility = symbol.DeclaredAccessibility.ToStorage(),
         Type = symbol.Kind.ToStorage(),
         Declarations = new StorageSlice<SymbolLocationDefinition>(-1, -1)
      };

      await batch.SymbolWriter.Write(deterministicId, symbolDefinition);
   }
}