using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Models.SymbolKeys;

/// <summary>
/// My own little version since rosyln's symbol key is internal :( 
/// </summary>
public partial record struct SymbolIdentifier : IEquatable<SymbolIdentifier>
{
   internal const int ProtocolVersion = 1;
   
   private readonly string? _value;
   private readonly int _protocolVersion;
   
   public SymbolIdentifier(string identifier, int protocolVersion = ProtocolVersion)
   {
      _value = identifier;
      _protocolVersion = protocolVersion;
   }

   public static SymbolIdentifier Create<TSymbol>(TSymbol symbol)
      where TSymbol : ISymbol
   {
      return new SymbolIdentifier();
   }

   public static bool CanCreate<TSymbol>(TSymbol symbol)
      where TSymbol : ISymbol
   {
      
      
      return true;
   }
}