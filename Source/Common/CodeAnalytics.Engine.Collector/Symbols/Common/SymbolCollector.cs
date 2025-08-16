using System.Security.Cryptography;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Extensions;
using CodeAnalytics.Engine.Collector.Formats;
using CodeAnalytics.Engine.Collector.Models.SymbolKeys;
using CodeAnalytics.Engine.Collector.Symbols.Interfaces;
using CodeAnalytics.Engine.Extensions.Hash;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Common;
using CodeAnalytics.Engine.Storage.Extensions;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Symbols.Common;

public sealed class SymbolCollector : ISymbolCollector<DbSymbol, ISymbol>
{
   public static async Task<DbSymbol?> Collect(ISymbol symbol, CollectContext context)
   {
      var symbolId = SymbolIdentifier.Create(symbol);

      return await context.DbMainContext.GetOrCreate(context.DbMainContext.Symbols)
         .Where(x => x.UniqueIdHash == symbolId.StringValue)
         .OnCreate(DbSymbolCreator)
         .Execute();
      
      DbSymbol DbSymbolCreator() =>
         new ()
         {
            Name = symbol.ToDisplayString(SymbolDisplayFormats.NameWithGenerics),
            MetadataName = symbol.MetadataName,
            FullPathName = GetFullPathName(symbol),
            Language = symbol.Language,
            AccessModifier = symbol.DeclaredAccessibility.ToAccessModifier(),
            Type = symbol.Kind.ToType(),
            UniqueId = symbolId.StringValue,
            UniqueIdHash = SHA1.CreateHexString(symbolId.StringValue),
            IsAbstract = symbol.IsAbstract,
            IsVirtual = symbol.IsVirtual,
            IsSealed = symbol.IsSealed,
            IsGenerated = symbol.IsImplicitlyDeclared,
            IsStatic = symbol.IsStatic,
            CreatedAt = DateTimeOffset.UtcNow,
         };
   }

   private static string GetFullPathName(ISymbol symbol)
   {
      return symbol switch
      {
         IMethodSymbol or IPropertySymbol or IFieldSymbol => 
            GetFullPathName(symbol.ContainingType.OriginalDefinition) 
            + "." + symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
         _ => symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
      };
   }
}