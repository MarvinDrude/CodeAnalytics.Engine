using Beskar.CodeAnalytics.Collector.Extensions;
using Beskar.CodeAnalytics.Collector.Identifiers;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Collector.Source;
using Beskar.CodeAnalytics.Collector.Symbols.Discovery;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Me.Memory.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
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
      var projectId = await ProjectDiscovery.Discover(batch, _handle);
      
      foreach (var tree in compilation.SyntaxTrees)
      {
         var semanticModel = compilation.GetSemanticModel(tree, ignoreAccessibility: true);
         var root = await tree.GetRootAsync(ct);

         var document = _handle.SolutionHandle.Solution.GetDocument(tree);
         var context = new DiscoverContext()
         {
            ProjectId = projectId,
            ProjectHandle = _handle,
            SemanticModel = semanticModel,
            SourceText = await tree.GetTextAsync(ct),
            SyntaxNode = root,
            SyntaxTree = tree,
            CancellationToken = ct,
            Document = document,
            DiscoveryBatch = batch
         };

         Dictionary<TextSpan, TextSpanCacheEntry> spans = [];
         
         foreach (var node in root.DescendantNodesAndSelf(descendIntoTrivia: false))
         {
            context.SyntaxNode = node;
            context.ResetSymbol();

            await HandleNode(spans, context);
         }
         
         // syntax file parsing
         await FileDiscovery.Discover(context, spans);
      }

      totalTimer.Dispose();
      LogStop(_handle.Project.Name, totalTimerResult.Elapsed);
   }

   private async Task HandleNode(Dictionary<TextSpan, TextSpanCacheEntry> spans , DiscoverContext context)
   {
      if (context.Symbol is not { } symbol
          || UniqueIdentifier.Create(symbol) is not { } uniqueIdentifier)
      {
         return;
      }

      var batch = context.DiscoveryBatch;

      var stringDefinition = batch.StringDefinitions.GetStringFileView(uniqueIdentifier);
      var deterministicId = batch.Identifiers.GenerateIdentifier(uniqueIdentifier, stringDefinition);

      spans[GetCorrectSpan(context.SyntaxNode)] = new TextSpanCacheEntry(deterministicId, IsSymbolFromSyntax(symbol, context.SyntaxNode));

      uint containingId = 0;
      var hasContaining = false;

      if (UniqueIdentifier.Create(symbol.ContainingSymbol) is { } containingIdentifier)
      {
         var cStringDefinition = batch.StringDefinitions.GetStringFileView(containingIdentifier);
         var cDeterministicId = batch.Identifiers.GenerateIdentifier(containingIdentifier, cStringDefinition);

         hasContaining = true;
         containingId = cDeterministicId;
      }

      var nameDef = batch.StringDefinitions.GetStringFileView(context.Symbol.Name);
      var metadataDef = batch.StringDefinitions.GetStringFileView(context.Symbol.MetadataName);
      var fullPathDef =
         batch.StringDefinitions.GetStringFileView(
            context.Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

      var symbolDefinition = new SymbolSpec()
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
         
         Declarations = new StorageView<SymbolLocationSpec>(-1, -1),
         Locations = new StorageView<SymbolLocationSpec>(-1, -1),
      };

      await batch.SymbolWriter.Write(deterministicId, symbolDefinition);
      await HandleSpecificSymbol(context, deterministicId);
   }

   private TextSpan GetCorrectSpan(SyntaxNode node) => node switch
   {
      // Declarations
      ClassDeclarationSyntax c => c.Identifier.Span,
      StructDeclarationSyntax s => s.Identifier.Span,
      InterfaceDeclarationSyntax i => i.Identifier.Span,
      EnumDeclarationSyntax e => e.Identifier.Span,
      MethodDeclarationSyntax m => m.Identifier.Span,
      PropertyDeclarationSyntax p => p.Identifier.Span,
      EventDeclarationSyntax e => e.Identifier.Span,
      VariableDeclaratorSyntax v => v.Identifier.Span,
      ParameterSyntax p => p.Identifier.Span,
      ConstructorDeclarationSyntax c => c.Identifier.Span,
      DestructorDeclarationSyntax d => d.Identifier.Span,
      EnumMemberDeclarationSyntax em => em.Identifier.Span,
      TypeParameterSyntax tp => tp.Identifier.Span,
      ArgumentSyntax a => GetCorrectSpan(a.Expression),

      // Usages & References
      IdentifierNameSyntax id => id.Identifier.Span,
      GenericNameSyntax g => g.Identifier.Span,
      AttributeSyntax a => a.Name.Span,
      ConstructorInitializerSyntax ci => ci.ThisOrBaseKeyword.Span,
      NameColonSyntax n => n.Name.Span,
   
      // Specialized Types
      SimpleBaseTypeSyntax sb => sb.Type.Span,
      QualifiedNameSyntax q => q.Right.Span,
      
      _ => node.Span,
   };

   private async Task<bool> HandleSpecificSymbol(DiscoverContext context, uint id)
   {
      return context.Symbol switch
      {
         IMethodSymbol => await MethodDiscovery.Discover(context, id),
         IParameterSymbol => await ParameterDiscovery.Discover(context, id),
         ITypeSymbol => await TypeDiscovery.Discover(context, id),
         IPropertySymbol => await PropertyDiscovery.Discover(context, id),
         IFieldSymbol => await FieldDiscovery.Discover(context, id),
         _ => false
      };
   }
   
   private bool IsSymbolFromSyntax(ISymbol symbol, SyntaxNode syntaxNode)
   {
      var nodeLocation = syntaxNode.GetLocation();
      
      foreach (var reference in symbol.DeclaringSyntaxReferences)
      {
         if (reference.GetSyntax().GetLocation().Equals(nodeLocation))
         {
            return true;
         }
      }
      
      return false;
   }
}