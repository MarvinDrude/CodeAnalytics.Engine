namespace CodeAnalytics.Engine.Collectors.Options;

public sealed class CollectorOptions
{
   public const string Prefix = "Collectors";
   
   public string Path { get; set; } = string.Empty;
   public string BasePath { get; set; } = string.Empty;
   public string OutputBasePath { get; set; } = string.Empty;
   
   public bool WriteSourceFiles { get; set; } = true;
}