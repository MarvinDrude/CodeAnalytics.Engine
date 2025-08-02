namespace CodeAnalytics.Web.Common.Models.Canvas.Commands;

public enum CommandKind : ushort
{
   BeginPath = 1,
   MoveTo = 2,
   LineTo = 3,
   ClosePath = 4,
   FillStyle = 5,
   Fill = 6,
   Arc = 7,
   GlobalCompositeOperation = 8,
   Rect = 9
}