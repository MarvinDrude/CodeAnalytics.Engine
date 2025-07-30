using CodeAnalytics.Engine.Contracts.Pipelines.Models;

namespace CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;

public interface IPipelineProvider
{
   public ValueTask<string> RunRawString(PipelineParameters parameters);

   public static virtual string Identifier { get; } = "Default";
}