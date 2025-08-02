using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Web.Common.Models.Canvas.Common;
using CodeAnalytics.Web.Common.Models.Canvas.Interfaces;

namespace CodeAnalytics.Web.Common.Models.Canvas.Commands;

[StructLayout(LayoutKind.Auto)]
public struct RectCommand : ICanvasCommand
{
   public CommandKind CommandKind => Kind;
   public const CommandKind Kind = CommandKind.Rect;

   public Rect Rect = Rect.Empty;

   public RectCommand()
   {
   }
   
   public void Serialize(ref ByteWriter writer)
   {
      writer.WriteLittleEndian(Rect.X);
      writer.WriteLittleEndian(Rect.Y);
      
      writer.WriteLittleEndian(Rect.Width);
      writer.WriteLittleEndian(Rect.Height);
   }
   
   public string GetJsFunction()
   {
      using var builder = new TextWriterSlim(stackalloc char[256]);
      builder.WriteLine("(ctx, dataView, offset) => {");

      builder.WriteLine("const x = dataView.getFloat32(offset, true); offset += 4;");
      builder.WriteLine("const y = dataView.getFloat32(offset, true); offset += 4;");
      
      builder.WriteLine("const width = dataView.getFloat32(offset, true); offset += 4;");
      builder.WriteLine("const height = dataView.getFloat32(offset, true); offset += 4;");

      builder.WriteLine("ctx.rect(x, y, width, height);");
      
      builder.WriteLine("return offset;");
      builder.WriteLine("}");
      
      return builder.ToString();
   }
}