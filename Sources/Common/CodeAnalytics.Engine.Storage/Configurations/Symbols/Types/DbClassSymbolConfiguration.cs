using CodeAnalytics.Engine.Storage.Configurations.Symbols.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Types;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAnalytics.Engine.Storage.Configurations.Symbols.Types;

public sealed class DbClassSymbolConfiguration
   : DbTypeSymbolBaseConfiguration<DbClassSymbol, DbClassSymbolId>
{
   public DbClassSymbolConfiguration() 
      : base(IdConverter)
   {
   }
   
   protected override void ConfigureInternal(EntityTypeBuilder<DbClassSymbol> builder)
   {
      base.ConfigureInternal(builder);

      builder.HasOne(x => x.BaseClassSymbol)
         .WithOne()
         .HasForeignKey<DbClassSymbol>(x => x.BaseClassSymbolId);
      
      builder.HasMany(x => x.ImplementedInterfaces)
         .WithMany(x => x.ImplementedByClass);
      
      builder.HasMany(x => x.ImplementedDirectInterfaces)
         .WithMany(x => x.ImplementedDirectByClass);
   }
   
   public static readonly ValueConverter<DbClassSymbolId, long> IdConverter = 
      new(id => id.Value, id => new DbClassSymbolId(id));
}