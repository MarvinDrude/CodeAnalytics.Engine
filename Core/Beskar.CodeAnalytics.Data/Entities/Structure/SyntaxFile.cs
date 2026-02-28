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

   public void Iterate(Action<ReadOnlySpan<char>, SyntaxTokenSpec> action)
   {
      foreach (var token in Tokens)
      {
         action(GetSpan(token), token);
      }
   }

   public string[] GetDebugTokens()
   {
      var result = new string[Tokens.Length];
      var counter = 0;
      
      Iterate((chars, token) =>
      {
         result[counter++] = $"\"{chars}\" | {token.Color} - {token.SymbolId} | {token.IsDeclaration}";
      });
      
      return result;
   }
}