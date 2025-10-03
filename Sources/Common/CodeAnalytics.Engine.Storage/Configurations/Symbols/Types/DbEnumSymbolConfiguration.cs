using CodeAnalytics.Engine.Storage.Configurations.Symbols.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Types;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAnalytics.Engine.Storage.Configurations.Symbols.Types;

public sealed class DbEnumSymbolConfiguration 
   : DbSymbolBaseConfiguration<DbEnumSymbol, DbEnumSymbolId>
{
   public DbEnumSymbolConfiguration() 
      : base(IdConverter)
   {
   }

   protected override void ConfigureInternal(EntityTypeBuilder<DbEnumSymbol> builder)
   {
      builder.HasOne(x => x.UnderlyingTypeSymbol)
         .WithMany()
         .HasForeignKey(x => x.UnderlyingTypeSymbolId);
   }

   public static readonly ValueConverter<DbEnumSymbolId, long> IdConverter = 
      new(id => id.Value, id => new DbEnumSymbolId(id));
}
