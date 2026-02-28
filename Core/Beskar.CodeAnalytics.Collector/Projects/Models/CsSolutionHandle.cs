using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Projects.Models;

public sealed record CsSolutionHandle(
   Workspace WorkSpace,
   Solution Solution)
   : IDisposable
{
   public void Dispose() => WorkSpace.Dispose();
}