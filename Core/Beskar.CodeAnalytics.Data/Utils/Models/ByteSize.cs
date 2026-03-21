using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Utils.Formatting;

namespace Beskar.CodeAnalytics.Data.Utils.Models;

public readonly record struct ByteSize(ulong Bytes)
{
   private static readonly string[] Suffixes = ["B", "KB", "MB", "GB", "TB", "PB", "EB"];
   private static readonly string[] FullSuffixes = ["Bytes", "Kilobytes", "Megabytes", "Gigabytes", "Terabytes", "Petabytes", "Exabytes"];

   public override string ToString()
   {
      return ToString(ByteSizeUnitFormat.Full);
   }

   public string ToString(ByteSizeUnitFormat format = ByteSizeUnitFormat.Short)
   {
      if (Bytes == 0)
      {
         return $"0 {GetSuffix(0, format)}";
      }
      
      var index = (int)Math.Floor(Math.Log(Bytes, 1024));
      index = Math.Min(index, Suffixes.Length - 1);
      
      var value = Math.Round(Bytes / Math.Pow(1024, index), 2);
      return $"{value.ToDotString()} {GetSuffix(index, format)}";
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private static string GetSuffix(int index, ByteSizeUnitFormat format)
   {
      return format is ByteSizeUnitFormat.Short 
         ? Suffixes[index] : FullSuffixes[index];
   }
}

public enum ByteSizeUnitFormat
{
   Short = 1,
   Full = 2
}