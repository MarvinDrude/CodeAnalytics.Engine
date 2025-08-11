using System.Runtime.CompilerServices;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;

namespace CodeAnalytics.Engine.Collector.Syntax.Interfaces;

public interface ISyntaxPredicator
{
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public bool Predicate(CollectContext context);
}