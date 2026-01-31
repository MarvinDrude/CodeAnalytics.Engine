using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Projects.Models;

public sealed record CsProjectHandle(
   CsSolutionHandle SolutionHandle,
   Project Project) 
   : IDisposable
{
   public Compilation Compilation => _compilation 
      ?? throw new InvalidOperationException("Compilation not initialized yet.");
   
   private Compilation? _compilation;
   
   public ValueTask<Compilation> GetCompilation(CancellationToken cancellationToken = default)
   {
      return _compilation is not null 
         ? ValueTask.FromResult(_compilation) 
         : GetCompilationAsync();

      async ValueTask<Compilation> GetCompilationAsync()
      {
         var compilation = await Project.GetCompilationAsync(cancellationToken);
         _compilation = compilation;

         return compilation ?? throw new InvalidOperationException("Compilation is null.");
      }
   }
   
   public void Dispose() => SolutionHandle.Dispose();
}