using System.Text;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Files;
using Me.Memory.Buffers;

namespace Beskar.CodeAnalytics.Data.Syntax.Readers;

public sealed class LinePreviewReader : IDisposable
{
   public ulong ByteCount => (ulong)_fileInfo.Length;
   
   private readonly FileInfo _fileInfo;
   private readonly MmfHandle _handle;

   public LinePreviewReader(string filePath)
   {
      _fileInfo = new FileInfo(filePath);
      _handle = new MmfHandle(filePath, writable: false);
   }

   public string GetString(LinePreviewView view)
   {
      using var buffer = _handle.GetBuffer();
      return buffer.GetString((long)view.Offset, view.Length, Encoding.UTF8);
   }

   public string[] GetStrings(scoped in ReadOnlySpan<LinePreviewView> views)
   {
      using var buffer = _handle.GetBuffer();
      using var owner = views.Length < 32
         ? new SpanOwner<(long Offset, int Length)>(stackalloc (long Offset, int Length)[views.Length])
         : new SpanOwner<(long Offset, int Length)>(views.Length);

      using var strs = buffer.GetStrings(owner.Span, Encoding.UTF8);
      return strs.WrittenSpan.ToArray();
   }
   
   public void Dispose()
   {
      _handle.Dispose();
   }
}