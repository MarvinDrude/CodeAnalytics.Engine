using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Collector.Extensions.Symbols;
using Me.Memory.Buffers;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Models.SymbolKeys;

/// <summary>
/// My own little version since rosyln's symbol key is internal :(
/// Only supports major symbols
/// </summary>
public readonly record struct SymbolIdentifier
{
   internal const int ProtocolVersion = 1;

   [field: AllowNull, MaybeNull]
   public string StringValue => field ?? throw new InvalidOperationException("Default struct.");

   private readonly int _protocolVersion;
   
   public SymbolIdentifier(string identifier, int protocolVersion = ProtocolVersion)
   {
      StringValue = identifier;
      _protocolVersion = protocolVersion;
   }

   public static SymbolIdentifier Create<TSymbol>(TSymbol symbol)
      where TSymbol : ISymbol
   {
      return new SymbolIdentifier(CreateString(symbol));
   }

   public static bool CanCreate<TSymbol>(TSymbol symbol)
      where TSymbol : ISymbol
   {
      if (!symbol.IsBodyLevel) return true;
      
      if (symbol.GetBodyLevelSourceLocations() is not { Length: > 0 } locations)
      {
         return false;
      }
         
      var compilation = ((ISourceAssemblySymbol)symbol.ContainingAssembly).Compilation;
      return compilation.SyntaxTrees.Contains(locations[0].SourceTree);
   }

   private static string CreateString<TSymbol>(TSymbol symbol)
      where TSymbol : ISymbol
   {
      var writer = new BufferWriter<char>();
      try
      {
         writer += ProtocolVersion.ToString();
         writer += '|';
         
         writer += symbol.Language;
         writer += '|';

         writer += symbol.ContainingAssembly.Identity.Name;
         writer += "|";

         if (symbol.GetDocumentationCommentId() is not { } seed)
         {
            seed = GetFallbackSeed(symbol);
         }
         writer += seed;
         
         return writer.WrittenSpan.ToString();
      }
      finally
      {
         writer.Dispose();
      }
   }

   private static string GetFallbackSeed<TSymbol>(TSymbol symbol)
      where TSymbol : ISymbol
   {
      var seed = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
      
      if (symbol is IMethodSymbol { MethodKind: MethodKind.LocalFunction })
      {
         seed = $"LF:{seed}";
      }

      return seed;
   }
}