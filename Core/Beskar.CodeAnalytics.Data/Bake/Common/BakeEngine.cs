using Beskar.CodeAnalytics.Data.Bake.Interfaces;
using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Hashing;

namespace Beskar.CodeAnalytics.Data.Bake.Common;

public sealed class BakeEngine
{
   private readonly List<IBakeStep> _steps = [];
   private readonly string _outputDirectory;
   
   public BakeEngine(
      string outputDirectory)
   {
      _outputDirectory = outputDirectory;
   }

   public async ValueTask Execute(CancellationToken ct = default)
   {
      using var context = new BakeContext()
      {
         OutputDirectoryPath = _outputDirectory,
         StringFileWriter = new (Path.Combine(_outputDirectory, "strpool.mmb")),
      };

      foreach (var step in _steps)
      {
         await step.Execute(context, ct);
      }
   }
   
   public BakeEngine AddStep<TStep>(TStep step) 
      where TStep : IBakeStep
   {
      _steps.Add(step);
      return this;
   }
}