using CodeAnalytics.Engine.Storage.Configurations.Symbols.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Members;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAnalytics.Engine.Storage.Configurations.Symbols.Members;

public sealed class DbPropertySymbolConfiguration
   : DbMemberSymbolBaseConfiguration<DbPropertySymbol, DbPropertySymbolId>
{
   public DbPropertySymbolConfiguration() 
      : base(IdConverter)
   {
   }

   protected override void ConfigureInternal(EntityTypeBuilder<DbPropertySymbol> builder)
   {
      base.ConfigureInternal(builder);

      builder.HasOne(x => x.TypeSymbol)
         .WithOne()
         .HasForeignKey<DbPropertySymbol>(x => x.TypeSymbolId);
      
      builder.HasOne(x => x.GetterSymbol)
         .WithOne()
         .HasForeignKey<DbPropertySymbol>(x => x.GetterSymbolId);
      
      builder.HasOne(x => x.SetterSymbol)
         .WithOne()
         .HasForeignKey<DbPropertySymbol>(x => x.SetterSymbolId);
      
      builder.HasOne(x => x.OverriddenSymbol)
         .WithOne()
         .HasForeignKey<DbPropertySymbol>(x => x.OverriddenSymbolId);
   }

   public static readonly ValueConverter<DbPropertySymbolId, long> IdConverter = 
      new(id => id.Value, id => new DbPropertySymbolId(id));
}