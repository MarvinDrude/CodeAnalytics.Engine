using CodeAnalytics.Engine.Storage.Models.Symbols.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAnalytics.Engine.Storage.Configurations.Symbols.Common;

public abstract class DbSymbolBaseConfiguration<TDbModel, TDbIdentifier> 
   : IEntityTypeConfiguration<TDbModel>
   where TDbIdentifier : struct
   where TDbModel : DbSymbolBase<TDbIdentifier>
{
   private readonly ValueConverter<TDbIdentifier, long> _idConverter;

   protected DbSymbolBaseConfiguration(ValueConverter<TDbIdentifier, long> idConverter)
   {
      _idConverter = idConverter;
   }
   
   protected abstract void ConfigureInternal(EntityTypeBuilder<TDbModel> builder);
   
   public void Configure(EntityTypeBuilder<TDbModel> builder)
   {
      builder.HasKey(x => x.Id);
      
      builder.Property(x => x.Id)
         .HasConversion(_idConverter)
         .ValueGeneratedOnAdd();
      
      builder.HasOne(x => x.Symbol)
         .WithOne()
         .HasForeignKey<TDbModel>(x => x.SymbolId);
      
      ConfigureInternal(builder);
   }
}