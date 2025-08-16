using System.ComponentModel;
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

         writer += GetDocId(symbol);
         
         return writer.WrittenSpan.ToString();
      }
      finally
      {
         writer.Dispose();
      }
   }

   private static string GetDocId<TSymbol>(TSymbol symbol)
      where TSymbol : ISymbol
   {
      if (!symbol.IsDefinition) 
         symbol = (TSymbol)symbol.OriginalDefinition;
      
      return symbol.Kind is SymbolKind.Parameter or SymbolKind.Local
         ? GetLocalDocId(symbol)
         : GetPublicDocId(symbol);
   }

   private static string GetLocalDocId<TSymbol>(TSymbol symbol)
      where TSymbol : ISymbol
   {
      return $"{GetDocId(symbol.ContainingSymbol)}#{symbol.MetadataName}";
   }

   private static string GetPublicDocId<TSymbol>(TSymbol symbol)
      where TSymbol : ISymbol
   {
      return symbol.GetDocumentationCommentId() 
             ?? throw new InvalidOperationException("DocId not found.");
   }
}