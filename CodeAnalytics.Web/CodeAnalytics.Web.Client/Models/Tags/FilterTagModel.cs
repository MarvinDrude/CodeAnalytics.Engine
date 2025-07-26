namespace CodeAnalytics.Web.Client.Models.Tags;

public sealed class FilterTagModel
{
   public required string Name { get; set; }
   
   public required string Value { get; set; }
   
   public bool IsActive { get; set; }
   
   public int Order { get; set; }
}