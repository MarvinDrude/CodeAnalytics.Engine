using Beskar.CodeAnalytics.Dashboard.Shared.Models.Syntax;

namespace Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Syntax;

public interface ILocationService
{
   public TokenLocationModel[] GetLocations(uint symbolId);
}