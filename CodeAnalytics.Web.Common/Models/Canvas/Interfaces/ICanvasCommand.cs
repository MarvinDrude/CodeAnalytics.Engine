using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Web.Common.Models.Canvas.Commands;

namespace CodeAnalytics.Web.Common.Models.Canvas.Interfaces;

public interface ICanvasCommand
{
   public CommandKind CommandKind { get; }

   public void Serialize(ref ByteWriter writer);
   
   public string GetJsFunction();
}