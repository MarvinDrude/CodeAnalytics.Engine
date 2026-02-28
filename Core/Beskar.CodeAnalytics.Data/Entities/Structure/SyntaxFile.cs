namespace Beskar.CodeAnalytics.Data.Entities.Structure;

public sealed class SyntaxFile
{
   public required uint FileId { get; set; }
   
   public required string FileName { get; set; }
   
   public required string RawText { get; set; }
   
   public required SyntaxTokenSpec[] Tokens { get; set; }

   public ReadOnlySpan<char> GetSpan(scoped in SyntaxTokenSpec token)
   {
      return RawText.AsSpan(token.Start, token.Length);
   }
}