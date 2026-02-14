using Beskar.CodeAnalytics.Data.Bake.Interfaces;
using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Discovery.Connectors;
using Me.Memory.Utils;

namespace Beskar.CodeAnalytics.Data.Bake.Steps;

public sealed class EdgeConnectionBakeStep : IBakeStep
{
   public string Name => "Edge Connections";

   public ValueTask Execute(BakeContext context, CancellationToken cancellationToken = default)
   {
      foreach (var (name, action) in Connectors)
      {
         var timeTook = TimeSpan.Zero;
         using (var timer = new StackTimer(ref timeTook))
         {
            action(context);
         }
         
         
      }
      
      TypeSymbolConnector.Connect(context);
      MethodSymbolConnector.Connect(context);
      
      
      return ValueTask.CompletedTask;
   }

   private static readonly (string Name, Action<BakeContext> Action)[] Connectors = [
      ("Type Symbols", TypeSymbolConnector.Connect)
   ];
}