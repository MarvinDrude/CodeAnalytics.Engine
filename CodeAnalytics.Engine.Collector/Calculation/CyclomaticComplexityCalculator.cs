using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FlowAnalysis;

namespace CodeAnalytics.Engine.Collector.Calculation;

public static class CyclomaticComplexityCalculator
{
   public static int Calculate(SyntaxNode node, SemanticModel model)
   {
      if (ControlFlowGraph.Create(node, model) is not { } cfg)
      {
         return 0;
      }
         
      var nodeCount = cfg.Blocks.Length;
      var edgeCount = cfg.Blocks.Sum(b => 
         (b.FallThroughSuccessor is not null ? 1 : 0) +
         (b.ConditionalSuccessor is not null ? 1 : 0));

      return edgeCount - nodeCount + 1;
   }
}