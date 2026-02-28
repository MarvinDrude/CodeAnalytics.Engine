using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Text;

namespace Beskar.CodeAnalytics.Collector.Source;

public sealed class SourceTokenizer(DiscoverContext context)
{
   private readonly DiscoverContext _context = context;

   public async Task<SyntaxFile> Tokenize(
      uint fileId,
      Dictionary<TextSpan, TextSpanCacheEntry> spans,
      CancellationToken cancellationToken)
   {
      var rawText = _context.SourceText.ToString();
      var all = await Classifier.GetClassifiedSpansAsync(
         _context.Document ?? throw new InvalidOperationException(),
         new TextSpan(0, _context.SourceText.Length),
         cancellationToken);
      var sortedSpans = all.OrderBy(s => s.TextSpan.Start)
         .ToArray();
      
      var tokens = new List<SyntaxTokenSpec>(sortedSpans.Length * 2);
      var pointer = 0;

      foreach (var span in sortedSpans)
      {
         if (span.TextSpan.Start > pointer)
         {
            
         }
      }

      return new SyntaxFile()
      {
         FileId = fileId,
         FileName = _context.Document.FilePath ?? "Unknown.cs",
         RawText = rawText,
         Tokens = tokens.ToArray()
      };
   }
}