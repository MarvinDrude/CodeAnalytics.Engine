using CodeAnalytics.Engine.Storage.Configurations.Symbols.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Types;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAnalytics.Engine.Storage.Configurations.Symbols.Types;

public sealed class DbStructSymbolConfiguration
   : DbTypeSymbolBaseConfiguration<DbStructSymbol, DbStructSymbolId>
{
   public DbStructSymbolConfiguration() 
      : base(IdConverter)
   {
   }
   
   protected override void ConfigureInternal(EntityTypeBuilder<DbStructSymbol> builder)
   {
      base.ConfigureInternal(builder);

      builder.HasMany(x => x.ImplementedInterfaces)
         .WithMany(x => x.ImplementedByStruct);
      
      builder.HasMany(x => x.ImplementedDirectInterfaces)
         .WithMany(x => x.ImplementedDirectByStruct);
   }
   
   public static readonly ValueConverter<DbStructSymbolId, long> IdConverter = 
      new(id => id.Value, id => new DbStructSymbolId(id));
}