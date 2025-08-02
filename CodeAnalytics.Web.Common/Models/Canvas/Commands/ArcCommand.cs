using System.Numerics;
using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Serialization.System.Common;
using CodeAnalytics.Web.Common.Models.Canvas.Interfaces;

namespace CodeAnalytics.Web.Common.Models.Canvas.Commands;

[StructLayout(LayoutKind.Auto)]
public struct ArcCommand : ICanvasCommand
{
   public CommandKind CommandKind => Kind;
   public const CommandKind Kind = CommandKind.Arc;

   public Vector2 Center = Vector2.Zero;
   
   public float Radius = 1f;
   public float StartAngle = 0f;
   public float EndAngle = 0f;
   
   public bool CounterClockwise = false;
   
   public ArcCommand()
   {
   }
   
   public void Serialize(ref ByteWriter writer)
   {
      writer.WriteLittleEndian(Center.X);
      writer.WriteLittleEndian(Center.Y);
      
      writer.WriteLittleEndian(Radius);
      writer.WriteLittleEndian(StartAngle);
      writer.WriteLittleEndian(EndAngle);
      
      writer.WriteLittleEndian((byte)(CounterClockwise ? 1 : 0));
   }
   
   public string GetJsFunction()
   {
      using var builder = new TextWriterSlim(stackalloc char[256]);
      builder.WriteLine("(ctx, dataView, offset) => {");

      builder.WriteLine("const x = dataView.getFloat32(offset, true); offset += 4;");
      builder.WriteLine("const y = dataView.getFloat32(offset, true); offset += 4;");
      
      builder.WriteLine("const radius = dataView.getFloat32(offset, true); offset += 4;");
      builder.WriteLine("const start = dataView.getFloat32(offset, true); offset += 4;");
      builder.WriteLine("const end = dataView.getFloat32(offset, true); offset += 4;");
      
      builder.WriteLine("const counterClockwise = dataView.getInt8(offset); offset += 1;");

      builder.WriteLine("ctx.arc(x, y, radius, start, end, counterClockwise);");
      
      builder.WriteLine("return offset;");
      builder.WriteLine("}");
      
      return builder.ToString();
   }
}