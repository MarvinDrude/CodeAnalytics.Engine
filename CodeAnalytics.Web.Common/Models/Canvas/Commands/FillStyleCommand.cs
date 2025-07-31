using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Serialization.System.Common;
using CodeAnalytics.Web.Common.Models.Canvas.Interfaces;

namespace CodeAnalytics.Web.Common.Models.Canvas.Commands;

[StructLayout(LayoutKind.Auto)]
public struct FillStyleCommand : ICanvasCommand
{
   public CommandKind CommandKind => Kind;
   public const CommandKind Kind = CommandKind.FillStyle;

   public string Style = string.Empty;

   public FillStyleCommand()
   {
   }
   
   public void Serialize(ref ByteWriter writer)
   {
      StringSerializer.Serialize(ref writer, ref Style);
   }
   
   public string GetJsFunction()
   {
      using var builder = new TextWriterSlim(stackalloc char[256]);
      builder.WriteLine("(ctx, dataView, offset) => {");

      builder.WriteLine("[offset, style] = dataView.getStringEx(offset);");
      builder.WriteLine("ctx.fillStyle = style;");
      
      builder.WriteLine("return offset;");
      builder.WriteLine("}");
      
      return builder.ToString();
   }
}