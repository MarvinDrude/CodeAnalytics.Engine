using CodeAnalytics.Engine.Storage.Configurations.Symbols.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Members;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAnalytics.Engine.Storage.Configurations.Symbols.Members;

public sealed class DbParameterSymbolConfiguration 
   : DbMemberSymbolBaseConfiguration<DbParameterSymbol, DbParameterSymbolId>
{
   public DbParameterSymbolConfiguration() 
      : base(IdConverter)
   {
   }

   protected override void ConfigureInternal(EntityTypeBuilder<DbParameterSymbol> builder)
   {
      base.ConfigureInternal(builder);

      builder.HasOne(x => x.TypeSymbol)
         .WithMany()
         .HasForeignKey(x => x.TypeSymbolId);
   }

   public static readonly ValueConverter<DbParameterSymbolId, long> IdConverter = 
      new(id => id.Value, id => new DbParameterSymbolId(id));
}