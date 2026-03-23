using Beskar.CodeAnalytics.Data.Analytics.Internal.Models;

namespace Beskar.CodeAnalytics.Data.Analytics.Internal.Interfaces;

public interface IProvider<out T>
   where T : allows ref struct
{
   public T GetValue(PipelineContext context);
}