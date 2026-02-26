
using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Extensions;
using Beskar.CodeAnalytics.Data.Files;
using Beskar.CodeAnalytics.Data.Hashing;
using Beskar.CodeAnalytics.Data.Indexes.Intermediate;
using Beskar.CodeAnalytics.Data.Indexes.Models;
using Beskar.CodeAnalytics.Data.Indexes.Readers;
using Beskar.CodeAnalytics.Data.Indexes.Search;
using Me.Memory.Utils;

var path = @"C:\Users\marvi\source\repos\Beskar.CodeAnalytics\Console\Beskar.CodeAnalytics.Collector.Console\bin\Debug\net11.0\Output\strpool.mmb";
using var strReader = new StringFileReader(path);
//
// var t = reader.GetAllStrings();

var indexPath = @"C:\Users\marvi\source\repos\Beskar.CodeAnalytics\Console\Beskar.CodeAnalytics.Collector.Console\bin\Debug\net11.0\Output\index_symbolspec.fullpathname_ngram.mmb";
var symbolPath = @"C:\Users\marvi\source\repos\Beskar.CodeAnalytics\Console\Beskar.CodeAnalytics.Collector.Console\bin\Debug\net11.0\Output\symbolspec.sorted.mmb";


var methodPath = @"C:\Users\marvi\source\repos\Beskar.CodeAnalytics\Console\Beskar.CodeAnalytics.Collector.Console\bin\Debug\net11.0\Output\methodsymbolspec.sorted.mmb";
var methodParaCountIndex = @"C:\Users\marvi\source\repos\Beskar.CodeAnalytics\Console\Beskar.CodeAnalytics.Collector.Console\bin\Debug\net11.0\Output\index_methodsymbolspec.parameter_count_staticwidebtree.mmb";

//using var reader = new NGramIndexReader(indexPath);
using var reader = new BTreeIndexReader<int>(methodParaCountIndex, Comparer<int>.Create((x, y) => x - y));

var time = TimeSpan.Zero;
IndexSearchResult<uint> res;
using (new StackTimer(ref time))
{
   // res = reader.Search(new NGramSearchQuery()
   // {
   //    Text = "ary",
   //    QueryType = NGramSearchQueryType.Contains,
   //    Limit = 1
   // });
   res = reader.Search(new BTreeSearchQuery<int>()
   {
      Keys = [1],
      Type = BTreeSearchQueryType.ExactMatch,
   });
}

Console.WriteLine(time);

// using var targetHandle = new MmfHandle(symbolPath, writable: false);
// using var buffer = targetHandle.GetBuffer();
// var count = (int)(targetHandle.Length / Unsafe.SizeOf<SymbolSpec>());
// var span = buffer.GetSpan<SymbolSpec>(0, count);
//
// foreach (var resId in res.Span)
// {
//    var index = span.BinaryFindIndex(resId);
//    ref var spec = ref span[index];
//
//    var fullPath = strReader.GetString(spec.FullPathName);
//    Console.WriteLine(fullPath);
// }

using var targetHandle = new MmfHandle(symbolPath, writable: false);
using var buffer = targetHandle.GetBuffer();
var count = (int)(targetHandle.Length / Unsafe.SizeOf<MethodSymbolSpec>());
var span = buffer.GetSpan<MethodSymbolSpec>(0, count);

foreach (var resId in res.Span)
{
   var index = span.BinaryFindIndex(resId);
   ref var spec = ref span[index];

   //var fullPath = strReader.GetString(spec.FullPathName);
   Console.WriteLine(spec.Parameters.Count);
}

Console.WriteLine();
