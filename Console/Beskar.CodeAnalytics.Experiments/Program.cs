
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Files;
using Beskar.CodeAnalytics.Data.Indexes.Intermediate;

var str = "";
var engrams = NGramHelper.CreateNGrams<NGram3>(str, 20, 3);

Console.WriteLine();
