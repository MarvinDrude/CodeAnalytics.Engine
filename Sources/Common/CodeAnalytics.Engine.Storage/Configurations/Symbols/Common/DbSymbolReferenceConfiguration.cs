using CodeAnalytics.Engine.Storage.Models.Symbols.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAnalytics.Engine.Storage.Configurations.Symbols.Common;

public sealed class DbSymbolReferenceConfiguration : IEntityTypeConfiguration<DbSymbolReference>
{
   public void Configure(EntityTypeBuilder<DbSymbolReference> builder)
   {
      builder.HasKey(x => x.Id);
      
      builder.Property(x => x.Id)
         .HasConversion(IdConverter)
         .ValueGeneratedOnAdd();

      builder.HasOne(x => x.Symbol)
         .WithMany(x => x.References)
         .HasForeignKey(x => x.SymbolId);

      builder.HasOne(x => x.File)
         .WithMany(x => x.SymbolReferences)
         .HasForeignKey(x => x.FileId);
   }
   
   public static readonly ValueConverter<DbSymbolReferenceId, long> IdConverter = 
      new(id => id.Value, id => new DbSymbolReferenceId(id));
}