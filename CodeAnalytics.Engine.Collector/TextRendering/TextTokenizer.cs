using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.TextRendering.Themes;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.TextRendering;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Text;

namespace CodeAnalytics.Engine.Collector.TextRendering;

public sealed class TextTokenizer
{
   private readonly CollectContext _context;
   private readonly CodeTheme _theme;

   public TextTokenizer(
      CollectContext context,
      CodeTheme theme)
   {
      _context = context;
      _theme = theme;
   }

   public async Task<PooledList<SyntaxSpan>> Tokenize(CancellationToken ct = default)
   {
      var lineCount = _context.SourceText.Lines.Count;
      var list = new PooledList<SyntaxSpan>(lineCount * 3);

      for (var e = 0; e < lineCount; e++)
      {
         var line = _context.SourceText.Lines[e];
         var lineSpan = line.Span;
         var classified = (await Classifier.GetClassifiedSpansAsync(_context.Document, lineSpan, ct))
            .GroupBy(x => x.TextSpan)
            .Select(g => g.OrderByDescending(c => c.ClassificationType == ClassificationTypeNames.Keyword).First())
            .OrderBy(c => c.TextSpan.Start)
            .ToList();
         
         TokenizeLine(ref list, lineSpan, classified);
         list.Add(new SyntaxSpan(string.Empty, string.Empty, isLineBreak: true));
      }

      return list;
   }

   private void TokenizeLine(
      ref PooledList<SyntaxSpan> list, 
      TextSpan lineSpan, 
      List<ClassifiedSpan> classifiedSpans)
   {
      var start = lineSpan.Start;
      
      if (classifiedSpans.Count == 0)
      {
         list.Add(new SyntaxSpan(GetText(lineSpan), GetColor()));
         return;
      }

      foreach (var classifiedSpan in classifiedSpans)
      {
         if (classifiedSpan.TextSpan.Intersection(lineSpan) 
             is not { } classified)
         {
            continue;
         }
         
         if (classified.Start > start)
         {
            var unclassified = new TextSpan(start, classified.Start - start);
            start = classified.Start;
            
            list.Add(new SyntaxSpan(GetText(unclassified), GetColor()));
         }
         
         var type = classifiedSpan.ClassificationType;
         var syntaxSpan = new SyntaxSpan(GetText(classifiedSpan.TextSpan), GetColor(type));
         
         list.Add(syntaxSpan);
         start = classified.End;
      }

      if (start >= lineSpan.End) return;
      
      var ending = new TextSpan(start, lineSpan.End - start);
      list.Add(new SyntaxSpan(GetText(ending), GetColor()));
   }
   
   private string GetText(TextSpan span)
   {
      return _context.SourceText.ToString(span);
   }

   private string GetColor(string? type = null)
   {
      return _theme.Colors.GetValueOrDefault(type ?? string.Empty)
         ?? _theme.Colors[CodeTheme.DefaultColorKeyName];
   }
}