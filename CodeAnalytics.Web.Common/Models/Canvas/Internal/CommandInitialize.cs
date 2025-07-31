namespace CodeAnalytics.Web.Common.Models.Canvas.Internal;

public sealed class CommandInitialize
{
   public required int RawKindValue { get; set; }
   
   public required string JsFunction { get; set; }
}