using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Pipelines.Models;

public class PipelineParameters
{
   public HashSet<StringId> Projects { get; set; } = [];
}