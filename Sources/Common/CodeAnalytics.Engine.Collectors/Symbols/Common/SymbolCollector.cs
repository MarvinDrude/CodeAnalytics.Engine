using System.Security.Cryptography;
using CodeAnalytics.Engine.Collectors.Extensions;
using CodeAnalytics.Engine.Collectors.Formats;
using CodeAnalytics.Engine.Collectors.Models.Contexts;
using CodeAnalytics.Engine.Collectors.Symbols.Interfaces;
using CodeAnalytics.Engine.Extensions.Hash;
using CodeAnalytics.Engine.Storage.Extensions;
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collectors.Symbols.Common;

public sealed class SymbolCollector<TSymbol> : ISymbolCollector<DbSymbol, TSymbol>
   where TSymbol : ISymbol
{
   public static async Task<DbSymbol?> Collect(TSymbol symbol, CollectContext context)
   {
      var symbolId = SymbolIdentifier.Create(symbol);
      var symbolIdHash = symbolId.CreateHash();

      return await context.DbContext.UpdateOrCreate(context.DbContext.Symbols)
         .Match(x => x.UniqueIdHash == symbolIdHash)
         .OnCreate(DbSymbolCreator)
         .Execute();

      DbSymbol DbSymbolCreator() =>
         new()
         {
            Name = symbol.ToDisplayString(SymbolDisplayFormats.NameWithGenerics),
            MetadataName = symbol.MetadataName,
            FullPathName = GetFullPathName(symbol),
            Language = symbol.Language,
            AccessModifier = symbol.DeclaredAccessibility.ToAccessModifier(),
            Type = symbol.Kind.ToType(),
            UniqueId = symbolId.StringValue,
            UniqueIdHash = symbolIdHash,
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