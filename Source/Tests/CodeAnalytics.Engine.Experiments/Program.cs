
using System.Security.Cryptography;
using CodeAnalytics.Engine.Storage.Contexts;
using CodeAnalytics.Engine.Storage.Enums.Components;
using CodeAnalytics.Engine.Storage.Models.Components.Common;

await using var ctx = new DbMainContext();

for (var e = 0; e < 500_000; e++)
{
   var comp = new SymbolComponent()
   {
      FullPathName = RandomNumberGenerator.GetHexString(22),
      MetadataName = RandomNumberGenerator.GetHexString(16),
      Name = RandomNumberGenerator.GetHexString(6),
      Kind = ComponentKind.Class,
      NodeHash = RandomNumberGenerator.GetHexString(12),
      SymbolDeclarations = []
   };

   await ctx.SymbolComponents.AddAsync(comp);
}

await ctx.SaveChangesAsync();


