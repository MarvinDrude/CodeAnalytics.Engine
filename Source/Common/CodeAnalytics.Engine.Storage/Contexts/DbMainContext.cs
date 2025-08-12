using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Contexts;

public sealed class DbMainContext : DbContext
{
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