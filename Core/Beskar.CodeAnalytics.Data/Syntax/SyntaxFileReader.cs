using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Files;
using Beskar.CodeAnalytics.Data.Serialization;
using Beskar.CodeAnalytics.Data.Syntax.Headers;

namespace Beskar.CodeAnalytics.Data.Syntax;

public sealed class SyntaxFileReader : IDisposable
{
   private readonly MmfHandle _handle;

   private readonly ulong _dictionaryOffset = sizeof(ulong);
   private readonly ulong _dataOffset;
   
   public SyntaxFileReader(string filePath)
   {
      _handle = new MmfHandle(filePath, writable: false);
      
      using var buffer = _handle.GetBuffer();
      _dataOffset = buffer.ReadUInt64LittleEndian(0);
   }

   public SyntaxFile? GetById(uint fileId)
   {
      using var buffer = _handle.GetBuffer();
      
      var offset = GetOffset(buffer, fileId);
      if (offset.Offset == 0) return null;
      
      var span = buffer.GetSpan<byte>((long)_dataOffset + (long)offset.Offset, offset.Length);
      return SyntaxFileSerializer.Deserialize(span);
   }

   private SyntaxDictionaryEntry GetOffset(MmfBuffer buffer, uint fileId)
   {
      var byteLength = _dataOffset - _dictionaryOffset;
      var count = byteLength / (ulong)Unsafe.SizeOf<SyntaxDictionaryEntry>();
      var span = buffer.GetSpan<SyntaxDictionaryEntry>((long)_dictionaryOffset, (int)count);
      
      var low = 0;
      var high = (int)count - 1;

      while (low <= high)
      {
         var mid = low + ((high - low) >> 1);
         var midFileId = span[mid].FileId;

         if (midFileId == fileId)
         {
            return span[mid];
         }

         if (midFileId < fileId)
         {
            low = mid + 1;
         }
         else
         {
            high = mid - 1;
         }
      }

      return new SyntaxDictionaryEntry()
      {
         Offset = 0
      };
   }
   
   public void Dispose()
   {
      _handle.Dispose();
   }
}