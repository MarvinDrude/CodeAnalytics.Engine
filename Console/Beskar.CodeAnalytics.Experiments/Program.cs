
using Beskar.CodeAnalytics.Storage.Discovery.Writers;
using Beskar.CodeAnalytics.Storage.Entities.Symbols;

using var writer = new SymbolDiscoveryWriter("");

for (var e = 0; e < 20_000_000; e++)
{
   await writer.Write(new SymbolDefinition()
   {
      Id = (ulong)e
   });
}

Console.WriteLine();
