using CodeAnalytics.Engine.Storage.Models.Symbols.Common;

namespace CodeAnalytics.Engine.Storage.Models.Symbols.Types;

public sealed class DbTypeInterfaceImplementation
{
   public required DbSymbolId ImplementedById { get; set; }
   public required DbSymbol ImplementedBy { get; set; }
   
   public required DbInterfaceSymbolId InterfaceId { get; set; }
   public required DbInterfaceSymbol Interface { get; set; }
}