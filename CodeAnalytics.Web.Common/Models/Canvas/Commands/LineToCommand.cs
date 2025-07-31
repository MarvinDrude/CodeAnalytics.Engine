using System.Numerics;
using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Web.Common.Models.Canvas.Interfaces;

namespace CodeAnalytics.Web.Common.Models.Canvas.Commands;

[StructLayout(LayoutKind.Auto)]
public struct LineToCommand : ICanvasCommand
{
   public CommandKind CommandKind => Kind;
   public const CommandKind Kind = CommandKind.LineTo;
   
   public Vector2 Position = Vector2.Zero;

   public LineToCommand()
   {
   }
   
   public void Serialize(ref ByteWriter writer)
   {
      writer.WriteLittleEndian(Position.X);
      writer.WriteLittleEndian(Position.Y);
   }
   
   public string GetJsFunction()
   {
      using var builder = new TextWriterSlim(stackalloc char[256]);
      builder.WriteLine("(ctx, dataView, offset) => {");

      builder.WriteLine("const x = dataView.getFloat32(offset, true); offset += 4;");
      builder.WriteLine("const y = dataView.getFloat32(offset, true); offset += 4;");

      builder.WriteLine("ctx.lineTo(x, y);");
      
      builder.WriteLine("return offset;");
      builder.WriteLine("}");
      
      return builder.ToString();
   }
}