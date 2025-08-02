using CodeAnalytics.Web.Common.Models.Canvas.Commands;
using CodeAnalytics.Web.Common.Models.Canvas.Interfaces;
using CodeAnalytics.Web.Common.Models.Canvas.Internal;

namespace CodeAnalytics.Web.Common.Models.Canvas;

public static class CommandMapper
{
   public static List<CommandInitialize> GetInitializeArray()
   {
      List<CommandInitialize> result = [];

      foreach (var command in AvailableCommands)
      {
         result.Add(new CommandInitialize()
         {
            RawKindValue = (int)command.CommandKind,
            JsFunction = command.GetJsFunction()
         });
      }
      
      return result;
   }
   
   private static readonly List<ICanvasCommand> AvailableCommands = [
      new BeginPathCommand(),
      new ClosePathCommand(),
      new LineToCommand(),
      new MoveToCommand(),
      new FillStyleCommand(),
      new FillCommand(),
      new ArcCommand(),
      new GlobalCompositeOperationCommand(),
      new RectCommand()
   ];
}