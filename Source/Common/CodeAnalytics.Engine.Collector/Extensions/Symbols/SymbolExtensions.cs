using System.Security.Cryptography;
using CodeAnalytics.Engine.Collector.Models.SymbolKeys;
using CodeAnalytics.Engine.Extensions.Hash;
using Me.Memory.Buffers;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Extensions.Symbols;

public static class SymbolExtensions
{
   extension<TSymbol>(TSymbol symbol)
      where TSymbol : ISymbol
   {
      public string GenerateUniqueId()
      {
         return SymbolIdentifier.Create(symbol).StringValue;
      }

      public string GeneratedUniqueIdHash()
      {
         return SHA1.CreateHexString(symbol.GenerateUniqueId());
      }

      public bool IsBodyLevel => symbol switch
      {
         ILabelSymbol 
            or IRangeVariableSymbol
            or ILocalSymbol 
            or IMethodSymbol { MethodKind: MethodKind.LocalFunction } => true,
         _ => false
      };

      public Location[] GetBodyLevelSourceLocations(CancellationToken ct = default)
      {
         using var writer = new BufferWriter<Location>(6);

         foreach (var location in symbol.Locations)
         {
            if (location.IsInSource)
            {
               writer.Add(location);
            }
         }

         foreach (var syntaxReference in symbol.DeclaringSyntaxReferences)
         {
            writer.Add(syntaxReference.GetSyntax(ct).GetLocation());
         }
         
         return writer.WrittenSpan.ToArray();
      }
   }
}