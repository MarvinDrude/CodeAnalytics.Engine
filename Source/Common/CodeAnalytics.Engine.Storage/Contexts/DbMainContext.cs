using CodeAnalytics.Engine.Storage.Entities.Symbols.Common;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Members;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Types;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Contexts;

public sealed class DbMainContext : DbContext
{
   public DbSet<DbSymbol> Symbols { get; set; } = null!;
   
   public DbSet<DbFieldSymbol> FieldSymbols { get; set; } = null!;
   public DbSet<DbMethodSymbol> MethodSymbols { get; set; } = null!;
   public DbSet<DbPropertySymbol> PropertySymbols { get; set; } = null!;
   public DbSet<DbParameterSymbol> ParameterSymbols { get; set; } = null!;
   
   public DbSet<DbClassSymbol> ClassSymbols { get; set; } = null!;
   public DbSet<DbInterfaceSymbol> InterfaceSymbols { get; set; } = null!;
   public DbSet<DbEnumSymbol> EnumSymbols { get; set; } = null!;
   public DbSet<DbStructSymbol> StructSymbols { get; set; } = null!;
   
   public DbSet<DbMemberInterfaceImplementation> MemberInterfaceImplementations { get; set; } = null!;
   public DbSet<DbTypeInterface> TypeInterfaces { get; set; } = null!;
   
   private readonly string? _connectionString;
   
   public DbMainContext(string? connectionString = null)
   {
      _connectionString = connectionString;
   }
   
   protected override void OnModelCreating(ModelBuilder builder)
   {
      
   }

   protected override void OnConfiguring(DbContextOptionsBuilder builder)
   {
      builder.UseNpgsql(_connectionString 
         ?? @"Server=localhost;Port=5432;Database=code_analytics;User ID=admin;Password=test");
   }
}