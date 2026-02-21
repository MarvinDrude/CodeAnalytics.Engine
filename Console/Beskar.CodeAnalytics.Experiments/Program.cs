
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Files;
using Beskar.CodeAnalytics.Data.Hashing;
using Beskar.CodeAnalytics.Data.Indexes.Intermediate;
using Beskar.CodeAnalytics.Data.Indexes.Readers;
using Beskar.CodeAnalytics.Data.Indexes.Search;

var path = @"C:\Users\marvi\source\repos\Beskar.CodeAnalytics\Console\Beskar.CodeAnalytics.Collector.Console\bin\Debug\net11.0\Output\strpool.mmb";
// var str = "362836";
// using var reader = new StringFileReader(path);
//
// var t = reader.GetAllStrings();

var indexPath = @"C:\Users\marvi\source\repos\Beskar.CodeAnalytics\Console\Beskar.CodeAnalytics.Collector.Console\bin\Debug\net11.0\Output\index_symbolspec.fullpathname_ngram.mmb";
using var reader = new NGramIndexReader(indexPath);

reader.Search(new NGramSearchQuery()
{
   Text = "Test"
});

Console.WriteLine();
