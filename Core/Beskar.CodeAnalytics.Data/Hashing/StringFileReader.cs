using System.Text;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Files;
using Me.Memory.Buffers;

namespace Beskar.CodeAnalytics.Data.Hashing;

public sealed class StringFileReader : IDisposable
{
   public ulong ByteCount => (ulong)_fileInfo.Length;
   
   private readonly FileInfo _fileInfo;
   private readonly MmfHandle _handle;

   public StringFileReader(string filePath)
   {
      _fileInfo = new FileInfo(filePath);
      _handle = new MmfHandle(filePath, writable: false);
   }

   public string GetString(StringFileView view)
   {
      return GetString(view.Offset);
   }

   public string GetString(ulong offset)
   {
      if (offset >= (ulong)_handle.Length)
      {
         throw new InvalidOperationException(
            $"String pool offset {offset} is out of bounds for file length {_handle.Length}. " +
            "The reader may have been initialized before the writer finished flushing.");
      }
      
      using var buffer = _handle.GetBuffer();
      
      var length = buffer.ReadInt32LittleEndian((long)offset);
      offset += sizeof(int);

      var span = buffer.GetSpan<byte>((long)offset, length);
      return Encoding.UTF8.GetString(span);
   }

   public Dictionary<StringFileView, string> GetStrings(scoped in ReadOnlySpan<StringFileView> views)
   {
      Dictionary<StringFileView, string> result = [];
      using var buffer = _handle.GetBuffer();
      
      foreach (var view in views)
      {
         var offset = (long)view.Offset;
         
         var length = buffer.ReadInt32LittleEndian(offset);
         offset += sizeof(int);
         
         var span = buffer.GetSpan<byte>(offset, length);
         result[view] = Encoding.UTF8.GetString(span);
      }
      
      return result;
   }

   public Dictionary<StringFileView, string> GetStrings<T>(scoped in ReadOnlySpan<T> entities, Func<T, ulong> getOffset)
   {
      using var orderOwner = entities.Length < 64 
         ? new SpanOwner<ulong>(stackalloc ulong[entities.Length]) 
         : new SpanOwner<ulong>(entities.Length);

      for (var index = 0; index < entities.Length; index++)
      {
         var entity = entities[index];
         var offset = getOffset(entity);
         
         orderOwner.Span[index] = offset;
      }
      
      orderOwner.Span.Sort(static (x, y) => x.CompareTo(y));

      Dictionary<StringFileView, string> result = [];
      using var buffer = _handle.GetBuffer();

      foreach (var offset in orderOwner.Span)
      {
         var longOffset = (long)offset;
         
         var length = buffer.ReadInt32LittleEndian(longOffset);
         longOffset += sizeof(int);
         
         var span = buffer.GetSpan<byte>(longOffset, length);
         result[new StringFileView(offset)] = Encoding.UTF8.GetString(span);
      }
      
      return result;
   }

   /// <summary>
   /// Only for debugging purposes.
   /// </summary>
   public Dictionary<ulong, string> GetAllStrings()
   {
      Dictionary<ulong, string> result = [];
      using var buffer = _handle.GetBuffer();
      var offset = 0UL;

      while (offset < (ulong)_handle.Length)
      {
         var length = buffer.ReadInt32LittleEndian((long)offset);
         offset += sizeof(int);

         var span = buffer.GetSpan<byte>((long)offset, length);
         var str = Encoding.UTF8.GetString(span);

         result[offset - sizeof(int)] = str;
         offset += (uint)length;
      }

      return result;
   }

   public void Dispose()
   {
      _handle.Dispose();
   }
}