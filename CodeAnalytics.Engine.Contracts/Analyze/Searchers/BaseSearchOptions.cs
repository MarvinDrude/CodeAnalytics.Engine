namespace CodeAnalytics.Engine.Contracts.Analyze.Searchers;

public class BaseSearchOptions
{
   public int MaxResults { get; set; } = 100;
   
   public string SearchText { get; set; } = string.Empty;
   
   public bool CaseSensitive { get; set; } = false;
   public bool TrimResults { get; set; } = true;
   
   public bool Structs { get; set; } = true;
   public bool Interfaces  { get; set; } = true;
   public bool Enums { get; set; } = true;
   public bool Classes  { get; set; } = true;
   
   public bool Constructors  { get; set; } = true;
   public bool Fields  { get; set; } = true;
   public bool Properties  { get; set; } = true;
   public bool Methods  { get; set; } = true;
}