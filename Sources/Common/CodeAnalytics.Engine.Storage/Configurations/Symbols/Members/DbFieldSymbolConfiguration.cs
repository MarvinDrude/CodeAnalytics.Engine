using CodeAnalytics.Engine.Storage.Configurations.Symbols.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Members;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAnalytics.Engine.Storage.Configurations.Symbols.Members;

public sealed class DbFieldSymbolConfiguration 
   : DbMemberSymbolBaseConfiguration<DbFieldSymbol, DbFieldSymbolId>
{
   public DbFieldSymbolConfiguration() 
      : base(IdConverter)
   {
   }

   protected override void ConfigureInternal(EntityTypeBuilder<DbFieldSymbol> builder)
   {
      base.ConfigureInternal(builder);

      builder.HasOne(x => x.TypeSymbol)
         .WithOne()
         .HasForeignKey<DbFieldSymbol>(x => x.TypeSymbolId);
   }

   public static readonly ValueConverter<DbFieldSymbolId, long> IdConverter = 
      new(id => id.Value, id => new DbFieldSymbolId(id));
}