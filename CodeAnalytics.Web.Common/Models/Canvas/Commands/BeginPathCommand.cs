using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Web.Common.Models.Canvas.Interfaces;

namespace CodeAnalytics.Web.Common.Models.Canvas.Commands;

[StructLayout(LayoutKind.Auto)]
public struct BeginPathCommand : ICanvasCommand
{
   public CommandKind CommandKind => Kind;
   public const CommandKind Kind = CommandKind.BeginPath;

   public BeginPathCommand()
   {
      
   }

   public void Serialize(ref ByteWriter writer)
   {
      // no extra serialize
   }
   
   public string GetJsFunction()
   {
      using var builder = new TextWriterSlim(stackalloc char[256]);
      builder.WriteLine("(ctx, dataView, offset) => {");
      builder.WriteLine("ctx.beginPath();");
      builder.WriteLine("return offset;");
      builder.WriteLine("}");
      
      return builder.ToString();
   }
}