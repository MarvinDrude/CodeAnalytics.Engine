using Beskar.CodeAnalytics.Collector.Extensions;
using Beskar.CodeAnalytics.Collector.Identifiers;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Collector.Symbols.Discovery;
using Beskar.CodeAnalytics.Storage.Entities.Misc;
using Beskar.CodeAnalytics.Storage.Entities.Symbols;
using Me.Memory.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Beskar.CodeAnalytics.Collector.Projects;

public sealed partial class ProjectCollector(
   CsProjectHandle handle,
   ILogger<ProjectCollector> logger)
{
   private readonly ILogger<ProjectCollector> _logger = logger;
   private readonly CsProjectHandle _handle = handle;

   public async Task Discover(DiscoveryBatch batch, CancellationToken ct = default)
   {
      LogStart(_handle.Project.Name);
      var totalTimerResult = new AsyncTimerResult();
      var totalTimer = new AsyncTimer(totalTimerResult);

      var timeResult = new AsyncTimerResult();
      Compilation compilation;

      using (new AsyncTimer(timeResult))
      {
         compilation = await _handle.GetCompilation(ct);
      }

      LogCompilationTime(_handle.Project.Name, timeResult.Elapsed);

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

      totalTimer.Dispose();
      LogStop(_handle.Project.Name, totalTimerResult.Elapsed);
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
      var fullPathDef =
         batch.StringDefinitions.GetStringDefinition(
            context.Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

      var symbolDefinition = new SymbolDefinition()
      {
         Id = deterministicId,
         ContainingId = containingId,
         HasContaining = hasContaining,

         Name = nameDef,
         MetadataName = metadataDef,
         FullPathName = fullPathDef,
         FullPathUniqueId = stringDefinition,

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
      await HandleSpecificSymbol(context, deterministicId);
   }

   private async Task<bool> HandleSpecificSymbol(DiscoverContext context, ulong id)
   {
      return context.Symbol switch
      {
         IMethodSymbol => await MethodDiscovery.Discover(context, id),
         IParameterSymbol => await ParameterDiscovery.Discover(context, id),
         ITypeSymbol => await TypeDiscovery.Discover(context, id),
         _ => false
      };
   }
}