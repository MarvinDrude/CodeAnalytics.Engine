
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Files;

const string path = @"C:\Users\marvi\source\repos\Beskar.CodeAnalytics\Console\Beskar.CodeAnalytics.Collector.Console\bin\Debug\net10.0\Output\symbolspec.discovery.sorted.mmb";

using var handle = new MmfHandle(path, false);
using var buffer = handle.GetBuffer();

var test = buffer.GetSpan<SymbolSpec>(0, 24);

Console.WriteLine();
