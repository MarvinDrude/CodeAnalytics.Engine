using Me.Memory.Buffers;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.System;

public sealed class DateTimeOffsetSerializer : ISerializer<DateTimeOffset>
{
   public void Write(ref ByteWriter writer, ref DateTimeOffset value)
   {
      writer.WriteLittleEndian(value.Ticks);
      writer.WriteLittleEndian((short)value.Offset.TotalMinutes);
   }

   public bool TryRead(ref ByteReader reader, out DateTimeOffset value)
   {
      var ticks = reader.ReadLittleEndian<long>();
      var offset = TimeSpan.FromMinutes(reader.ReadLittleEndian<short>());
      
      value = new DateTimeOffset(ticks, offset);
      return true;
   }

   public int CalculateByteLength(ref DateTimeOffset value)
   {
      return sizeof(long) + sizeof(short);
   }
}