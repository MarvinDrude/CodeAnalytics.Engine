using System.Numerics;
using System.Threading.Channels;
using CodeAnalytics.Web.Common.Models.Canvas.Commands;
using CodeAnalytics.Web.Common.Models.Canvas.Interfaces;

namespace CodeAnalytics.Web.Common.Models.Canvas;

public sealed class CanvasContext
{
   private readonly Channel<ICanvasCommand> _commands;

   public int Width { get; set; }
   public int Height { get; set; }
   
   public CanvasContext(Channel<ICanvasCommand> commands)
   {
      _commands = commands;
   }

   public void BeginPath() => Add(new BeginPathCommand());
   public void ClosePath() => Add(new ClosePathCommand());

   public void MoveTo(Vector2 pos) => Add(new MoveToCommand() { Position = pos });
   public void MoveTo(float x, float y) => MoveTo(new Vector2(x, y));
   
   public void LineTo(Vector2 pos) => Add(new LineToCommand() { Position = pos });
   public void LineTo(float x, float y) => LineTo(new Vector2(x, y));
   
   public void FillStyle(string style) => Add(new FillStyleCommand() { Style = style });
   public void Fill() => Add(new FillCommand());

   public void Arc(Vector2 center, float radius, float startAngle, float endAngle, bool counterClockwise)
      => Add(new ArcCommand() { Center = center, Radius = radius, StartAngle = startAngle, EndAngle = endAngle, CounterClockwise = counterClockwise });
   public void Arc(float cx, float cy, float radius, float startAngle, float endAngle, bool counterClockwise)
      => Add(new ArcCommand() { Center = new Vector2(cx, cy), Radius = radius, StartAngle = startAngle, EndAngle = endAngle, CounterClockwise = counterClockwise });
   
   public void GlobalCompositeOperation(string setName) => Add(new GlobalCompositeOperationCommand() { SetName = setName });
   
   private void Add<TCommand>(TCommand command) 
      where TCommand : ICanvasCommand
   {
      _commands.Writer.TryWrite(command);
   }
}