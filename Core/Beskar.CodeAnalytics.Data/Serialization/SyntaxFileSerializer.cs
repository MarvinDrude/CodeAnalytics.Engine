using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Extensions;
using Me.Memory.Buffers;
using Me.Memory.Serialization.Formatters.Common;

namespace Beskar.CodeAnalytics.Data.Serialization;

public static class SyntaxFileSerializer
{
   private static readonly StringSerializer StringSerializer = new ();
   
   public static void Serialize(SyntaxFile syntaxFile, Action<ReadOnlySpan<byte>> action)
   {
      var builder = new ByteWriter(stackalloc byte[1024]);

      try
      {
         builder.WriteLittleEndian(0); // reserve total length
         builder.WriteLittleEndian(syntaxFile.FileId);

         var fileName = syntaxFile.FileName;
         StringSerializer.Write(ref builder, ref fileName);

         var rawText = syntaxFile.RawText;
         StringSerializer.Write(ref builder, ref rawText);

         builder.WriteLittleEndian(syntaxFile.Tokens.Length);

         foreach (ref var token in syntaxFile.Tokens.AsSpan())
         {
            builder.WriteBytes(token.AsBytes());
         }

         var positionBefore = builder.Position;
         builder.Position = 0;
         builder.WriteLittleEndian(positionBefore - sizeof(int));

         builder.Position = positionBefore;
         action(builder.WrittenSpan);
      }
      finally
      {
         builder.Dispose();
      }
   }

   public static SyntaxFile Deserialize(scoped in Span<byte> bytes)
   {
      var reader = new ByteReader(bytes);
      var fileId = reader.ReadLittleEndian<uint>();

      if (!StringSerializer.TryRead(ref reader, out var fileName)
          || !StringSerializer.TryRead(ref reader, out var rawText))
      {
         throw new InvalidOperationException("Memory layout unexpected.");
      }

      var tokenSize = reader.ReadLittleEndian<int>();
      var tokenBytes = reader.ReadBytes(Unsafe.SizeOf<SyntaxTokenSpec>() * tokenSize);
      var tokens = new SyntaxTokenSpec[tokenSize];
      
      MemoryMarshal.Cast<byte, SyntaxTokenSpec>(tokenBytes).CopyTo(tokens);

      return new SyntaxFile()
      {
         FileId = fileId,
         FileName = fileName,
         RawText = rawText,
         Tokens = tokens
      };
   }
}