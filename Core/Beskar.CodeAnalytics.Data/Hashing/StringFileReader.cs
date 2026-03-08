using System.Text;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Files;
using Me.Memory.Buffers;

namespace Beskar.CodeAnalytics.Data.Hashing;

public sealed class StringFileReader : IDisposable
{
   private readonly MmfHandle _handle;

   public StringFileReader(string filePath)
   {
      // Ensure we see the latest size on disk
      _ = new FileInfo(filePath);
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