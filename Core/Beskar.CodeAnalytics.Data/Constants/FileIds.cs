namespace Beskar.CodeAnalytics.Data.Constants;

public static class FileIds
{
   public static readonly FileId StringPool = new("##string-pool##");
   
   public static readonly FileId Symbol = new("##symbol##");
   public static readonly FileId TypeSymbol = new("##type-symbol##");
   public static readonly FileId NamedTypeSymbol = new("##named-type-symbol##");
   public static readonly FileId ParameterSymbol = new("##parameter-symbol##");
   public static readonly FileId TypeParameterSymbol = new("##type-parameter-symbol##");
   public static readonly FileId MethodSymbol = new("##method-symbol##");
   public static readonly FileId FieldSymbol = new("##field-symbol##");
   public static readonly FileId PropertySymbol = new("##property-symbol##");
   public static readonly FileId EdgeSymbol = new("##edge-symbol##");
}

public readonly record struct FileId(string Value);