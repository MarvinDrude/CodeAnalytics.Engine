namespace CodeAnalytics.Engine.Collector.TextRendering.Themes;

public sealed partial class CodeTheme
{
   public const string DefaultColorKeyName = "default-default";
   
   public Dictionary<string, string> Colors { get; set; } = [];
}