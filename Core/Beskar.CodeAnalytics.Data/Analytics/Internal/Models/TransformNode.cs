using Beskar.CodeAnalytics.Data.Analytics.Internal.Interfaces;

namespace Beskar.CodeAnalytics.Data.Analytics.Internal.Models;

public class TransformNode<TInput, TOutput>(
   IProvider<TInput> parent,
   Func<TInput, TOutput> transform)
   : IProvider<TOutput>
   where TInput : IEquatable<TInput>
   where TOutput : IEquatable<TOutput>
{
   private readonly IProvider<TInput> _parent = parent;
   private readonly Func<TInput, TOutput> _transform = transform;

   private TInput? _lastInput;
   private TOutput? _lastOutput;
   
   public TOutput GetValue(PipelineContext context)
   {
      context.TotalCallCount++;
      var input = _parent.GetValue(context);

      if (_lastInput is not null && _lastInput.Equals(input))
      {
         context.CacheHitCount++;
         return _lastOutput ?? throw new InvalidOperationException("Last output should not be null");
      }
      
      _lastInput = input;
      _lastOutput = _transform(input);

      return _lastOutput;
   }
}