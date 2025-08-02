using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Serialization.System.Common;
using CodeAnalytics.Web.Common.Models.Canvas.Interfaces;

namespace CodeAnalytics.Web.Common.Models.Canvas.Commands;

[StructLayout(LayoutKind.Auto)]
public struct GlobalCompositeOperationCommand : ICanvasCommand
{
   public CommandKind CommandKind => Kind;
   public const CommandKind Kind = CommandKind.GlobalCompositeOperation;

   public string SetName = string.Empty;

   public GlobalCompositeOperationCommand()
   {
   }
   
   public void Serialize(ref ByteWriter writer)
   {
      StringSerializer.Serialize(ref writer, ref SetName);
   }
   
   public string GetJsFunction()
   {
      using var builder = new TextWriterSlim(stackalloc char[256]);
      builder.WriteLine("(ctx, dataView, offset) => {");

      builder.WriteLine("[offset, style] = dataView.getStringEx(offset);");
      builder.WriteLine("ctx.globalCompositeOperation = style;");
      
      builder.WriteLine("return offset;");
      builder.WriteLine("}");
      
      return builder.ToString();
   }
}