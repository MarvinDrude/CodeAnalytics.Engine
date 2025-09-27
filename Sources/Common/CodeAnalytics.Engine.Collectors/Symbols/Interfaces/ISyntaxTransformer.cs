using CodeAnalytics.Engine.Collectors.Models.Contexts;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collectors.Symbols.Interfaces;

public interface ISyntaxTransformer<in TSyntaxNode>
   where TSyntaxNode : SyntaxNode
{
   public static abstract Task<bool> TryTransform(TSyntaxNode node, CollectContext context);
}