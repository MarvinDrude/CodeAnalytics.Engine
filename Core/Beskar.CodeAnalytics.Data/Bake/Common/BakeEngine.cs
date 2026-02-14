using Beskar.CodeAnalytics.Data.Bake.Interfaces;
using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Hashing;
using Me.Memory.Threading;
using Me.Memory.Utils;
using Microsoft.Extensions.Logging;

namespace Beskar.CodeAnalytics.Data.Bake.Common;

public sealed class BakeEngine
{
   private readonly List<IBakeStep> _steps = [];
   private readonly string _outputDirectory;

   private readonly ILogger<BakeEngine> _logger;
   
   public BakeEngine(
      string outputDirectory,
      ILoggerFactory loggerFactory)
   {
      _outputDirectory = outputDirectory;
      _logger = loggerFactory.CreateLogger<BakeEngine>();
   }

   public async ValueTask Execute(
      StringFileWriter stringFileWriter,
      WorkPool workPool,
      Dictionary<FileId, string> fileNames,
      bool deleteIntermediateFiles,
      CancellationToken ct = default)
   {
      await using var context = new BakeContext()
      {
         OutputDirectoryPath = _outputDirectory,
         StringFileWriter = stringFileWriter,
         DeleteIntermediateFiles = deleteIntermediateFiles,
         WorkPool = workPool,
         FileNames = fileNames
      };

      foreach (var step in _steps)
      {
         var timerResult = new AsyncTimerResult();
         using (var timer = new AsyncTimer(timerResult))
         {
            await step.Execute(context, ct);
         }
      }
   }
   
   public BakeEngine AddStep<TStep>(TStep step) 
      where TStep : IBakeStep
   {
      _steps.Add(step);
      return this;
   }
}