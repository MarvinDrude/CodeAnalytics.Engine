using System.Text;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Files;

namespace Beskar.CodeAnalytics.Data.Hashing;

public sealed class StringFileReader : IDisposable
{
   private readonly MmfHandle _handle;

   public StringFileReader(string filePath)
   {
      _handle = new MmfHandle(filePath, writable: false);
   }

   public string GetString(StringFileView view)
   {
      return GetString(view.Offset);
   }

   public string GetString(ulong offset)
   {
      using var buffer = _handle.GetBuffer();
      
      var length = buffer.ReadInt32LittleEndian((long)offset);
      offset += sizeof(int);

      var span = buffer.GetSpan<byte>((long)offset, length);
      return Encoding.UTF8.GetString(span);
   }

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