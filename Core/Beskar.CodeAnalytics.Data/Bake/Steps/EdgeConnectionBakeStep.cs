using Beskar.CodeAnalytics.Data.Bake.Interfaces;
using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Discovery.Connectors;
using Me.Memory.Utils;
using Microsoft.Extensions.Logging;

namespace Beskar.CodeAnalytics.Data.Bake.Steps;

public sealed class EdgeConnectionBakeStep : IBakeStep
{
   public string Name => "Edge Connections";

   private readonly ILogger<EdgeConnectionBakeStep> _logger;
   
   public EdgeConnectionBakeStep(ILoggerFactory loggerFactory)
   {
      _logger = loggerFactory.CreateLogger<EdgeConnectionBakeStep>();
   }

   public ValueTask Execute(BakeContext context, CancellationToken cancellationToken = default)
   {
      foreach (var (name, action) in Connectors)
      {
         var timeTook = TimeSpan.Zero;
         using (new StackTimer(ref timeTook))
         {
            action(context);
         }
      }
      
      return ValueTask.CompletedTask;
   }

   private static readonly (string Name, Action<BakeContext> Action)[] Connectors = [
      ("Type Symbols", TypeSymbolConnector.Connect),
      ("Method Symbols", MethodSymbolConnector.Connect),
      ("Named Type Symbols", NamedTypeSymbolConnector.Connect),
      ("Type Parameter Symbols", TypeParameterConnector.Connect),
   ];
}