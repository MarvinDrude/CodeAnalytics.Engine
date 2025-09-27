using CodeAnalytics.Engine.Storage.Configurations.Structure;
using CodeAnalytics.Engine.Storage.Configurations.Symbols.Common;
using CodeAnalytics.Engine.Storage.Configurations.Symbols.Members;
using CodeAnalytics.Engine.Storage.Configurations.Symbols.Types;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Common;

public sealed partial class DbMainContext
{
   protected override void OnModelCreating(ModelBuilder builder)
   {
      builder.ApplyConfiguration(new DbSolutionConfiguration());
      builder.ApplyConfiguration(new DbProjectConfiguration());
      builder.ApplyConfiguration(new DbFileConfiguration());

      builder.ApplyConfiguration(new DbSymbolConfiguration());
      builder.ApplyConfiguration(new DbSymbolReferenceConfiguration());
      
      builder.ApplyConfiguration(new DbFieldSymbolConfiguration());
      builder.ApplyConfiguration(new DbMethodSymbolConfiguration());
      builder.ApplyConfiguration(new DbParameterSymbolConfiguration());
      builder.ApplyConfiguration(new DbPropertySymbolConfiguration());

      builder.ApplyConfiguration(new DbEnumSymbolConfiguration());
      builder.ApplyConfiguration(new DbClassSymbolConfiguration());
      builder.ApplyConfiguration(new DbStructSymbolConfiguration());
      builder.ApplyConfiguration(new DbInterfaceSymbolConfiguration());

      foreach (var foreignKey in builder.Model
                  .GetEntityTypes()
                  .SelectMany(e => e.GetForeignKeys()))
      {
         foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
      }
   }
}