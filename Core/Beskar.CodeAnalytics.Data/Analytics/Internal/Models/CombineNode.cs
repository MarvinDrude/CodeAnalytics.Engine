using Beskar.CodeAnalytics.Data.Analytics.Internal.Interfaces;

namespace Beskar.CodeAnalytics.Data.Analytics.Internal.Models;

public class CombineNode<TLeft, TRight>(
   IProvider<TLeft> left,
   IProvider<TRight> right) 
   : IProvider<(TLeft, TRight)>
   where TLeft : IEquatable<TLeft>
   where TRight : IEquatable<TRight>
{
   public (TLeft, TRight) GetValue(PipelineContext context)
   {
      return (left.GetValue(context), right.GetValue(context));
   }
}