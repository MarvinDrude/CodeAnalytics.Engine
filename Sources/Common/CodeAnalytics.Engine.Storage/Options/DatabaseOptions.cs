namespace CodeAnalytics.Engine.Storage.Options;

public sealed class DatabaseOptions
{
   public const string Prefix = "Database";
   
   public string? ConnectionString { get; set; }
}