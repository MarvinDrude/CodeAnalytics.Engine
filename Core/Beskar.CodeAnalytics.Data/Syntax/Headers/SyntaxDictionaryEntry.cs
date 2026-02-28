using System.Runtime.InteropServices;

namespace Beskar.CodeAnalytics.Data.Syntax.Headers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SyntaxDictionaryEntry
{
   public uint FileId;
   
   public ulong Offset;
   public int Length;
   
   public static readonly IComparer<SyntaxDictionaryEntry> Comparer 
      = Comparer<SyntaxDictionaryEntry>.Create((x, y) => x.FileId.CompareTo(y.FileId));
}