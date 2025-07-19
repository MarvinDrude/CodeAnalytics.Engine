using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CodeAnalytics.Engine.Common.Buffers;

[StructLayout(LayoutKind.Auto)]
public ref struct TextWriterSlim
{
   public ReadOnlySpan<char> WrittenSpan => _buffer.WrittenSpan;
   
   private BufferWriter<char> _buffer;

   public TextWriterSlim(Span<char> buffer)
   {
      _buffer = new BufferWriter<char>(buffer);
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void WriteText(scoped in ReadOnlySpan<char> text)
   {
      _buffer.Write(text);
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void Write(char cha)
   {
      _buffer.Add(cha);
   }
   
   public void Write(scoped ReadOnlySpan<char> text, bool multiLine = false)
   {
      if (!multiLine)
      {
         WriteText(text);
      }
      else
      {
         while (text.Length > 0)
         {
            var newLinePos = text.IndexOf('\n');

            if (newLinePos >= 0)
            {
               var line = text[..newLinePos];
               
               WriteIf(!line.IsEmpty, line);
               WriteLine();

               text = text[(newLinePos + 1)..];
            }
            else
            {
               WriteText(text);
               break;
            }
         }
      }
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void WriteIf(bool condition, scoped ReadOnlySpan<char> content, bool multiLine = false)
   {
      if (condition)
      {
         Write(content, multiLine);
      }  
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void WriteLine()
   {
      _buffer.Add('\n');
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void WriteLineIf(bool condition)
   {
      if (condition)
      {
         WriteLine();
      }
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void WriteLineIf(bool condition, scoped ReadOnlySpan<char> content, bool multiLine = false)
   {
      if (condition)
      {
         WriteLine(content, multiLine);
      }
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void WriteLine(scoped Span<char> content, bool multiLine = false)
   {
      Write(content, multiLine);
      WriteLine();
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void WriteLine(scoped ReadOnlySpan<char> content, bool multiLine = false)
   {
      Write(content, multiLine);
      WriteLine();
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void WriteText(string text)
   {
      WriteText(text.AsSpan());
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void WriteText(scoped ReadOnlySpan<char> text)
   {
      _buffer.Write(text);
   }
   
   public void Dispose()
   {
      _buffer.Dispose();
   }
}