using CodeAnalytics.Engine.Storage.Configurations.Symbols.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Members;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAnalytics.Engine.Storage.Configurations.Symbols.Members;

public sealed class DbMethodSymbolConfiguration
   : DbMemberSymbolBaseConfiguration<DbMethodSymbol, DbMethodSymbolId>
{
   public DbMethodSymbolConfiguration() 
      : base(IdConverter)
   {
   }

   protected override void ConfigureInternal(EntityTypeBuilder<DbMethodSymbol> builder)
   {
      base.ConfigureInternal(builder);

      builder.HasOne(x => x.ReturnTypeSymbol)
         .WithOne()
         .HasForeignKey<DbMethodSymbol>(x => x.ReturnTypeSymbolId);
      
      builder.HasOne(x => x.OverriddenSymbol)
         .WithOne()
         .HasForeignKey<DbMethodSymbol>(x => x.OverriddenSymbolId);
   }

   public static readonly ValueConverter<DbMethodSymbolId, long> IdConverter = 
      new(id => id.Value, id => new DbMethodSymbolId(id));
}