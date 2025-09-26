using CodeAnalytics.Engine.Storage.Models.Symbols.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAnalytics.Engine.Storage.Configurations.Symbols.Common;

public abstract class DbTypeSymbolBaseConfiguration<TDbModel, TDbIdentifier>
   : DbSymbolBaseConfiguration<TDbModel, TDbIdentifier>
   where TDbIdentifier : struct
   where TDbModel : DbTypeSymbolBase<TDbIdentifier>
{
   protected DbTypeSymbolBaseConfiguration(ValueConverter<TDbIdentifier, long> idConverter) 
      : base(idConverter)
   {
   }
   
   protected override void ConfigureInternal(EntityTypeBuilder<TDbModel> builder)
   {
      
   }
}