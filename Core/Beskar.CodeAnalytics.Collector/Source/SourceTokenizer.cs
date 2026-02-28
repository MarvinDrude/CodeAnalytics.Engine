using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Enums.Syntax;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Text;

namespace Beskar.CodeAnalytics.Collector.Source;

public sealed class SourceTokenizer(
   DiscoverContext context,
   Dictionary<TextSpan, TextSpanCacheEntry> spans)
{
   private readonly DiscoverContext _context = context;
   private readonly Dictionary<TextSpan, TextSpanCacheEntry> _spans = spans;

   public async Task<SyntaxFile> Tokenize(
      uint fileId,
      CancellationToken cancellationToken)
   {
      var rawText = _context.SourceText.ToString();
      var all = await Classifier.GetClassifiedSpansAsync(
         _context.Document ?? throw new InvalidOperationException(),
         new TextSpan(0, _context.SourceText.Length),
         cancellationToken);
      
      var sortedSpans = all.GroupBy(x => x.TextSpan)
         .Select(g => g.OrderByDescending(c => c.ClassificationType == ClassificationTypeNames.Keyword).First())
         .OrderBy(c => c.TextSpan.Start)
         .ToArray();
      
      var tokens = new List<SyntaxTokenSpec>(sortedSpans.Length * 2);
      var pointer = 0;

      foreach (var span in sortedSpans)
      {
         if (span.TextSpan.Start > pointer)
         {
            HandleLines(tokens, pointer, span.TextSpan.Start - pointer);
         }
         
         HandleLines(tokens, span.TextSpan.Start, span.TextSpan.Length, span);
         pointer = span.TextSpan.End;
      }

      if (pointer < _context.SourceText.Length)
      {
         HandleLines(tokens, pointer, _context.SourceText.Length - pointer);
      }

      return new SyntaxFile()
      {
         FileId = fileId,
         FileName = _context.Document.FilePath ?? "Unknown.cs",
         RawText = rawText,
         Tokens = tokens.ToArray()
      };
   }

   private void HandleLines(List<SyntaxTokenSpec> tokens, int start, int length, ClassifiedSpan? classified = null)
   {
      var textSegment = _context.SourceText.ToString(new TextSpan(start, length));
      var currentPos = 0;
      
      while (currentPos < textSegment.Length)
      {
         var nextNewline = textSegment.IndexOfAny(['\r', '\n'], currentPos);

         if (nextNewline == -1)
         {
            // no newlines anymore - add rest
            tokens.Add(CreateToken(start + currentPos, textSegment.Length - currentPos, classified));
            break;
         }

         if (nextNewline > currentPos)
         {
            // text before newline
            tokens.Add(CreateToken(start + currentPos, nextNewline - currentPos, classified));
         }

         // Determine newline length (handle \r\n vs \n)
         var newlineLength = 1;
         if (textSegment[nextNewline] == '\r' 
             && nextNewline + 1 < textSegment.Length 
             && textSegment[nextNewline + 1] == '\n')
         {
            newlineLength = 2;
         }

         // Add the line break token
         var lbToken = CreateToken(start + nextNewline, newlineLength, null);
         lbToken.IsLineBreak = true;
         tokens.Add(lbToken);

         currentPos = nextNewline + newlineLength;
      }
   }

   private SyntaxTokenSpec CreateToken(int start, int length, ClassifiedSpan? classified)
   {
      var hasSymbol = false;
      TextSpanCacheEntry? entry = null;
      
      if (_spans.TryGetValue(new TextSpan(start, length), out var span))
      {
         hasSymbol = true;
         entry = span;
      }
      
      var spec = new SyntaxTokenSpec()
      {
         Start = start,
         Length = length,
         Color = classified is null ? SyntaxColor.Default : TokenColorizer.Determine(classified.Value),
         IsDeclaration = entry?.IsDeclaration ?? false,
         IsKeyword = classified?.ClassificationType == ClassificationTypeNames.Keyword,
         IsLineBreak = false,
         IsPlain = classified is null,
         HasSymbol = hasSymbol,
         SymbolId = span?.SymbolId ?? 0
      };

      return spec;
   }
}