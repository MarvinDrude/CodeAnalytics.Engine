using CodeAnalytics.Engine.Storage.Models.Structure;
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Members;
using CodeAnalytics.Engine.Storage.Models.Symbols.Types;
using CodeAnalytics.Engine.Storage.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CodeAnalytics.Engine.Storage.Common;

public sealed partial class DbMainContext : DbContext
{
   public DbSet<DbSolution> Solutions => Set<DbSolution>();
   public DbSet<DbProject> Projects => Set<DbProject>();
   public DbSet<DbFile> Files => Set<DbFile>();

   public DbSet<DbSymbol> Symbols => Set<DbSymbol>();
   public DbSet<DbSymbolReference> SymbolReferences => Set<DbSymbolReference>();
   
   public DbSet<DbFieldSymbol> FieldSymbols => Set<DbFieldSymbol>();
   public DbSet<DbMethodSymbol> MethodSymbols => Set<DbMethodSymbol>();
   public DbSet<DbParameterSymbol> ParameterSymbols => Set<DbParameterSymbol>();
   public DbSet<DbPropertySymbol> PropertySymbols => Set<DbPropertySymbol>();
   
   public DbSet<DbEnumSymbol> EnumSymbols => Set<DbEnumSymbol>();
   public DbSet<DbClassSymbol> ClassSymbols => Set<DbClassSymbol>();
   public DbSet<DbStructSymbol> StructSymbols => Set<DbStructSymbol>();
   public DbSet<DbInterfaceSymbol> InterfaceSymbols => Set<DbInterfaceSymbol>();
   
   private readonly IOptionsSnapshot<DatabaseOptions> _dbOptionsSnapshot;
   private DatabaseOptions DbOptions => _dbOptionsSnapshot.Value;
   
   public DbMainContext(IOptionsSnapshot<DatabaseOptions> dbOptionsSnapshot)
   {
      _dbOptionsSnapshot = dbOptionsSnapshot;
   }

   protected override void OnConfiguring(DbContextOptionsBuilder builder)
   {
      builder
         .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
         .EnableDetailedErrors();
      
      builder.UseNpgsql(DbOptions.ConnectionString 
         ?? throw new InvalidOperationException("No connection string."));
   }
}