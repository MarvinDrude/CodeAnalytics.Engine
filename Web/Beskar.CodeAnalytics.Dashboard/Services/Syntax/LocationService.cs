using Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Services;
using Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Syntax;

namespace Beskar.CodeAnalytics.Dashboard.Services.Syntax;

public sealed class LocationService(IDatabaseProvider provider) : ILocationService
{
   private readonly IDatabaseProvider _provider = provider;

   public void GetLocations(uint symbolId)
   {
      var db = _provider.GetDescriptor();
      var reader = db.Structure.SymbolLocations.GetReader();
      
      using var locations = reader.LeaseById(symbolId);
      
      
      
      _ = "";
   }
}