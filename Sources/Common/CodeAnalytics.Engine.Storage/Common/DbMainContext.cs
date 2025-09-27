using CodeAnalytics.Engine.Storage.Models.Structure;
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Members;
using CodeAnalytics.Engine.Storage.Models.Symbols.Types;
using CodeAnalytics.Engine.Storage.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

   private readonly ILogger<DbMainContext> _logger;
   private readonly ILoggerFactory _loggerFactory;
   
   private readonly IOptionsSnapshot<DatabaseOptions> _dbOptionsSnapshot;
   private DatabaseOptions DbOptions => _dbOptionsSnapshot.Value;
   
   public DbMainContext(
      IOptionsSnapshot<DatabaseOptions> dbOptionsSnapshot,
      ILogger<DbMainContext> logger,
      ILoggerFactory loggerFactory)
   {
      _dbOptionsSnapshot = dbOptionsSnapshot;
      _logger = logger;
      _loggerFactory = loggerFactory;
   }
   
   protected override void OnConfiguring(DbContextOptionsBuilder builder)
   {
      builder
         .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
         .EnableDetailedErrors()
         .EnableSensitiveDataLogging()
         .LogTo(
            log => _logger.LogDebug(log),
            [DbLoggerCategory.Database.Command.Name],
            LogLevel.Information);
      
      builder.UseNpgsql(DbOptions.ConnectionString 
         ?? throw new InvalidOperationException("No connection string."));
   }

   public async Task CleanData(CancellationToken ct = default)
   {
      await Solutions.ExecuteDeleteAsync(ct);
      await Projects.ExecuteDeleteAsync(ct);
      await Files.ExecuteDeleteAsync(ct);
      
      await Symbols.ExecuteDeleteAsync(ct);
      await SymbolReferences.ExecuteDeleteAsync(ct);
      
      await FieldSymbols.ExecuteDeleteAsync(ct);
      await MethodSymbols.ExecuteDeleteAsync(ct);
      await ParameterSymbols.ExecuteDeleteAsync(ct);
      await PropertySymbols.ExecuteDeleteAsync(ct);
      
      await EnumSymbols.ExecuteDeleteAsync(ct);
      await ClassSymbols.ExecuteDeleteAsync(ct);
      await StructSymbols.ExecuteDeleteAsync(ct);
      await InterfaceSymbols.ExecuteDeleteAsync(ct);
   }
}