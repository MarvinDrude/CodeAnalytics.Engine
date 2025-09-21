using CodeAnalytics.Engine.Storage.Configurations.Structure;
using CodeAnalytics.Engine.Storage.Configurations.Symbols.Common;
using CodeAnalytics.Engine.Storage.Configurations.Symbols.Members;
using CodeAnalytics.Engine.Storage.Configurations.Symbols.Types;
using CodeAnalytics.Engine.Storage.Models.Structure;
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Members;
using CodeAnalytics.Engine.Storage.Models.Symbols.Types;
using CodeAnalytics.Engine.Storage.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CodeAnalytics.Engine.Storage.Common;

public sealed class DbMainContext : DbContext
{
   public DbSet<DbSolution> Solutions => Set<DbSolution>();
   public DbSet<DbProject> Projects => Set<DbProject>();
   public DbSet<DbFile> Files => Set<DbFile>();

   public DbSet<DbSymbol> Symbols => Set<DbSymbol>();
   
   public DbSet<DbFieldSymbol> FieldSymbols => Set<DbFieldSymbol>();
   public DbSet<DbMethodSymbol> MethodSymbols => Set<DbMethodSymbol>();
   public DbSet<DbParameterSymbol> ParameterSymbols => Set<DbParameterSymbol>();
   public DbSet<DbPropertySymbol> PropertySymbols => Set<DbPropertySymbol>();
   
   public DbSet<DbEnumSymbol> EnumSymbols => Set<DbEnumSymbol>();
   
   private readonly IOptionsSnapshot<DatabaseOptions> _dbOptionsSnapshot;
   private DatabaseOptions DbOptions => _dbOptionsSnapshot.Value;
   
   public DbMainContext(IOptionsSnapshot<DatabaseOptions> dbOptionsSnapshot)
   {
      _dbOptionsSnapshot = dbOptionsSnapshot;
   }

   protected override void OnModelCreating(ModelBuilder builder)
   {
      builder.ApplyConfiguration(new DbSolutionConfiguration());
      builder.ApplyConfiguration(new DbProjectConfiguration());
      builder.ApplyConfiguration(new DbFileConfiguration());

      builder.ApplyConfiguration(new DbSymbolConfiguration());
      
      builder.ApplyConfiguration(new DbFieldSymbolConfiguration());
      builder.ApplyConfiguration(new DbMethodSymbolConfiguration());
      builder.ApplyConfiguration(new DbParameterSymbolConfiguration());
      builder.ApplyConfiguration(new DbPropertySymbolConfiguration());

      builder.ApplyConfiguration(new DbEnumSymbolConfiguration());
   }

   protected override void OnConfiguring(DbContextOptionsBuilder builder)
   {
      builder.UseNpgsql(DbOptions.ConnectionString 
         ?? throw new InvalidOperationException("No connection string."));
   }
}