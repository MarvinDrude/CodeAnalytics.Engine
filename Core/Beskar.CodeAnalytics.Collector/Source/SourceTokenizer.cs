using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Syntax;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Text;

namespace Beskar.CodeAnalytics.Collector.Source;

public sealed class SourceTokenizer(
   uint fileId,
   DiscoverContext context,
   Dictionary<TextSpan, TextSpanCacheEntry> spans)
{
   private readonly DiscoverContext _context = context;
   private readonly Dictionary<TextSpan, TextSpanCacheEntry> _spans = spans;
   private readonly uint _fileId = fileId;
   
   private int _lastLineIndex = -1;
   private LinePreviewView _lastPreviewView;

   public async Task<SyntaxFile> Tokenize(
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
      var lineNumber = 1;
      var pointer = 0;

      foreach (var span in sortedSpans)
      {
         if (span.TextSpan.Start > pointer)
         {
            lineNumber = await HandleLines(tokens, pointer, span.TextSpan.Start - pointer, lineNumber);
         }
         
         lineNumber = await HandleLines(tokens, span.TextSpan.Start, span.TextSpan.Length, lineNumber, span);
         pointer = span.TextSpan.End;
      }

      if (pointer < _context.SourceText.Length)
      {
         await HandleLines(tokens, pointer, _context.SourceText.Length - pointer, lineNumber);
      }

      return new SyntaxFile()
      {
         FileId = fileId,
         FileName = Path.GetFileName(_context.Document.FilePath ?? "Unknown.cs"),
         RawText = rawText,
         Tokens = tokens.ToArray()
      };
   }

   private async Task<int> HandleLines(List<SyntaxTokenSpec> tokens, int start, int length, int lineNumber, ClassifiedSpan? classified = null)
   {
      char[] lineBreaks = ['\r', '\n', '\u0085', '\u2028', '\u2029'];
      var textSegment = _context.SourceText.ToString(new TextSpan(start, length));
      var currentPos = 0;
      
      while (currentPos < textSegment.Length)
      {
         var nextNewline = textSegment.IndexOfAny(lineBreaks, currentPos);

         if (nextNewline == -1)
         {
            // no newlines anymore - add rest
            tokens.Add(await CreateToken(start + currentPos, textSegment.Length - currentPos, classified, lineNumber));
            break;
         }

         if (nextNewline > currentPos)
         {
            // text before newline
            tokens.Add(await CreateToken(start + currentPos, nextNewline - currentPos, classified, lineNumber));
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
         var lbToken = await CreateToken(start + nextNewline, newlineLength, null, lineNumber);
         lbToken.IsLineBreak = true;
         tokens.Add(lbToken);
         
         lineNumber++;
         currentPos = nextNewline + newlineLength;
      }
      
      return lineNumber;
   }

   private async Task<SyntaxTokenSpec> CreateToken(int start, int length, ClassifiedSpan? classified, int lineNumber)
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

      if (spec.HasSymbol)
      {
         var lineIndex = lineNumber - 1;
         var line = _context.SourceText.Lines[lineIndex];
         
         var lineText = line.ToString();
         var preview = await _context.DiscoveryBatch.LinePreviewWriter.Write(lineText);

         preview.TokenStart = start - line.Start;
         preview.TokenLength = length;
         
         var location = new SymbolLocationSpec()
         {
            SymbolId = spec.SymbolId,
            SourceFileId = _fileId,
            
            LineNumber = lineNumber,
            LinePreview = preview,
            
            IsDeclaration = spec.IsDeclaration
         };
         
         var task = _context.DiscoveryBatch.LocationWriter.Write(location.SymbolId, location);
         if (!task.IsCompletedSuccessfully) throw new InvalidOperationException();
      }

      return spec;
   }
}