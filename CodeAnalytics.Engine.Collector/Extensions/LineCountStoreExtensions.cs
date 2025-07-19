using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Contracts.Collectors;
using CodeAnalytics.Engine.Contracts.Ids;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CodeAnalytics.Engine.Collector.Extensions;

public static class LineCountStoreExtensions
{
   public static void CalculateLineStats(
      this LineCountStore store, NodeId id, SyntaxNode node, CollectContext context)
   {
      var text = context.SourceText;

      if (node.SyntaxTree.FilePath != context.SyntaxTree.FilePath)
      {
         return;
      }
      
      var syntaxPath = context.GetRelativePath(node.SyntaxTree.FilePath);
      var syntaxPathId = context.Store.StringIdStore.GetOrAdd(syntaxPath);

      if (!store.LineCountsPerFile.TryGetValue(syntaxPathId, out _))
      {
         store.LineCountsPerFile[syntaxPathId] = store.ParseLineStats(text, 0, text.Length);
      }

      var span = node.FullSpan;
      var stats = store.ParseLineStats(text, span.Start, span.End);

      store.AddToNode(id, syntaxPathId, stats);
   }

   public static LineCountStats ParseLineStats(this LineCountStore store, SourceText text, int start, int stop)
   {
      var stats = new LineCountStats();

      var startLine = text.Lines.GetLineFromPosition(start).LineNumber;
      var endLine = text.Lines.GetLineFromPosition(stop).LineNumber;
         
      for (var e = startLine; e <= endLine; e++)
      {
         var line = text.Lines[e];
         var lStart = line.Start;
         var lStop = line.End;

         var hasCode = false;

         for (var pos = lStart; pos < lStop; pos++)
         {
            var curr = text[pos];
            if (char.IsWhiteSpace(curr)) continue;

            hasCode = true;
            break;
         }

         if (hasCode)
         {
            stats.CodeCount++;
         }

         stats.LineCount++;
      }
      
      return stats;
   }
}