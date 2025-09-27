using CodeAnalytics.Engine.Storage.Configurations.Symbols.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Types;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAnalytics.Engine.Storage.Configurations.Symbols.Types;

public sealed class DbInterfaceSymbolConfiguration
   : DbTypeSymbolBaseConfiguration<DbInterfaceSymbol, DbInterfaceSymbolId>
{
   public DbInterfaceSymbolConfiguration() 
      : base(IdConverter)
   {
   }
   
   protected override void ConfigureInternal(EntityTypeBuilder<DbInterfaceSymbol> builder)
   {
      base.ConfigureInternal(builder);

      builder.HasMany(x => x.ImplementedInterfaces)
         .WithMany(x => x.ImplementedByInterface);
      
      builder.HasMany(x => x.ImplementedDirectInterfaces)
         .WithMany(x => x.ImplementedDirectByInterface);
   }
   
   public static readonly ValueConverter<DbInterfaceSymbolId, long> IdConverter = 
      new(id => id.Value, id => new DbInterfaceSymbolId(id));
}