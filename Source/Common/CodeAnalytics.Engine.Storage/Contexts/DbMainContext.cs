using CodeAnalytics.Engine.Storage.Models.Common;
using CodeAnalytics.Engine.Storage.Models.Components.Common;
using CodeAnalytics.Engine.Storage.Models.Components.Interfaces;
using CodeAnalytics.Engine.Storage.Models.Components.Members;
using CodeAnalytics.Engine.Storage.Models.Components.Types;
using CodeAnalytics.Engine.Storage.Models.Structure;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Contexts;

public sealed class DbMainContext : DbContext
{
   public DbSet<SolutionReference> SolutionReferences { get; set; } = null!;
   public DbSet<ProjectReference> ProjectReferences { get; set; } = null!;
   public DbSet<FileReference> FileReferences { get; set; } = null!;
   
   public DbSet<SymbolComponent> SymbolComponents { get; set; } = null!;
   public DbSet<SymbolDeclaration> SymbolDeclarations { get; set; } = null!;
   public DbSet<ParameterComponent> ParameterComponents { get; set; } = null!;
   
   public DbSet<FieldComponent> FieldComponents { get; set; } = null!;
   public DbSet<ConstructorComponent> ConstructorComponents { get; set; } = null!;
   public DbSet<MethodComponent> MethodComponents { get; set; } = null!;
   public DbSet<PropertyComponent> PropertyComponents { get; set; } = null!;
   public DbSet<EnumValueComponent> EnumValueComponents { get; set; } = null!;
   
   public DbSet<ClassComponent> ClassComponents { get; set; } = null!;
   public DbSet<EnumComponent> EnumComponents { get; set; } = null!;
   public DbSet<InterfaceComponent> InterfaceComponents { get; set; } = null!;
   public DbSet<StructComponent> StructComponents { get; set; } = null!;

   private readonly string? _connectionString;
   
   public DbMainContext(string? connectionString = null)
   {
      _connectionString = connectionString;
   }
   
   protected override void OnModelCreating(ModelBuilder builder)
   {
      CreateSymbolReference<ConstructorComponent>(builder);
      CreateSymbolReference<EnumValueComponent>(builder);
      CreateSymbolReference<FieldComponent>(builder);
      CreateSymbolReference<MethodComponent>(builder);
      CreateSymbolReference<PropertyComponent>(builder);
      CreateSymbolReference<ParameterComponent>(builder);
      
      CreateMemberContainingTypeReference<ConstructorComponent>(builder);
      CreateMemberContainingTypeReference<EnumValueComponent>(builder);
      CreateMemberContainingTypeReference<FieldComponent>(builder);
      CreateMemberContainingTypeReference<MethodComponent>(builder);
      CreateMemberContainingTypeReference<PropertyComponent>(builder);
      
      CreateSymbolReference<ClassComponent>(builder);
      CreateSymbolReference<InterfaceComponent>(builder);
      CreateSymbolReference<StructComponent>(builder);
      CreateSymbolReference<EnumComponent>(builder);
      
      CreateTypeReferences<ClassComponent>(builder);
      CreateTypeReferences<InterfaceComponent>(builder);
      CreateTypeReferences<StructComponent>(builder);
      
      builder.Entity<FieldComponent>()
         .HasOne(x => x.Type)
         .WithOne()
         .HasForeignKey<FieldComponent>(x => x.TypeId)
         .IsRequired(false);

      builder.Entity<ConstructorComponent>()
         .HasMany(x => x.ParameterComponents)
         .WithOne(x => x.ConstructorComponent)
         .HasForeignKey(x => x.ConstructorComponentId)
         .IsRequired(false);
      
      builder.Entity<MethodComponent>()
         .HasMany(x => x.ParameterComponents)
         .WithOne(x => x.MethodComponent)
         .HasForeignKey(x => x.MethodComponentId)
         .IsRequired(false);
      builder.Entity<MethodComponent>()
         .HasMany(x => x.InterfaceImplementations)
         .WithOne(x => x.InterfaceImplementation)
         .HasForeignKey(x => x.InterfaceImplementationId)
         .IsRequired(false);
      builder.Entity<MethodComponent>()
         .HasOne(x => x.OverriddenMethod)
         .WithOne(x => x.OverridingMethod)
         .HasForeignKey<MethodComponent>(x => x.OverriddenMethodId)
         .IsRequired(false);
      builder.Entity<MethodComponent>()
         .HasOne(x => x.Type)
         .WithOne()
         .HasForeignKey<MethodComponent>(x => x.TypeId)
         .IsRequired(false);
      
      builder.Entity<PropertyComponent>()
         .HasMany(x => x.InterfaceImplementations)
         .WithOne(x => x.InterfaceImplementation)
         .HasForeignKey(x => x.InterfaceImplementationId)
         .IsRequired(false);
      builder.Entity<PropertyComponent>()
         .HasOne(x => x.OverriddenProperty)
         .WithOne(x => x.OverridingProperty)
         .HasForeignKey<PropertyComponent>(x => x.OverriddenPropertyId)
         .IsRequired(false);
      
      builder.Entity<ClassComponent>()
         .HasOne(x => x.BaseClass)
         .WithOne(x => x.DerivedClass)
         .HasForeignKey<ClassComponent>(x => x.BaseClassId)
         .IsRequired(false);
   }

   protected override void OnConfiguring(DbContextOptionsBuilder builder)
   {
      builder.UseSqlite(_connectionString 
         ?? @"Data Source=C:\Users\marvi\RiderProjects2\Source\identifier.sqlite");
   }

   private void CreateTypeReferences<T>(ModelBuilder builder)
      where T : TypeComponent
   {
      builder.Entity<T>()
         .HasMany(x => x.Interfaces)
         .WithOne()
         .IsRequired(false);
      builder.Entity<T>()
         .HasMany(x => x.DirectInterfaces)
         .WithOne()
         .IsRequired(false);
      
      builder.Entity<T>()
         .HasMany(x => x.ConstructorComponents)
         .WithOne()
         .IsRequired(false);
      builder.Entity<T>()
         .HasMany(x => x.MethodComponents)
         .WithOne()
         .IsRequired(false);
      builder.Entity<T>()
         .HasMany(x => x.PropertyComponents)
         .WithOne()
         .IsRequired(false);
      builder.Entity<T>()
         .HasMany(x => x.FieldComponents)
         .WithOne()
         .IsRequired(false);
   }
   
   private void CreateSymbolReference<T>(ModelBuilder builder)
      where T : class, IComponent
   {
      builder.Entity<T>()
         .HasOne(x => x.SymbolComponent)
         .WithOne()
         .HasForeignKey<T>(x => x.SymbolComponentId)
         .IsRequired();
   }
   
   private void CreateMemberContainingTypeReference<T>(ModelBuilder builder)
      where T : class, IMemberComponent
   {
      builder.Entity<T>()
         .HasOne(x => x.ContainingType)
         .WithOne()
         .HasForeignKey<T>(x => x.ContainingTypeId)
         .IsRequired();
   }
}