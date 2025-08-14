
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Members;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Types;
using CodeAnalytics.Engine.Storage.Enums.Modifiers;
using CodeAnalytics.Engine.Storage.Enums.Symbols;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Entities.Symbols.Common;

[Index(nameof(Id), IsUnique = true)]
[Index(nameof(UniqueId), IsUnique = true)]
[Index(nameof(UniqueIdHash), IsUnique = true)]
public sealed class DbSymbol
{
   public long Id { get; set; }
   
   [MaxLength(3000)]
   public required string UniqueId { get; set; }
   [MaxLength(300)]
   public required string UniqueIdHash { get; set; }
   
   [MaxLength(1000)]
   public required string Name { get; set; }
   [MaxLength(1000)]
   public required string MetadataName { get; set; }
   [MaxLength(3000)]
   public required string FullPathName { get; set; }
   
   [MaxLength(60)]
   public required string Language { get; set; }
   
   public bool IsVirtual { get; set; }
   public bool IsAbstract { get; set; }
   public bool IsStatic { get; set; }
   public bool IsSealed { get; set; }
   public bool IsGenerated { get; set; }
   
   public AccessModifier AccessModifier { get; set; } = AccessModifier.NotApplicable;
   public required SymbolType Type { get; set; }
   
   public DateTimeOffset CreatedAt { get; set; }
   
   [ForeignKey(nameof(ContainingSymbolId))]
   public DbSymbol? ContainingTypeSymbol { get; set; }
   public long ContainingSymbolId { get; set; }

   [InverseProperty(nameof(DbSymbolReference.Symbol))]
   public List<DbSymbolReference> References { get; set; } = [];
   
   [InverseProperty(nameof(DbMemberInterfaceImplementation.Symbol))]
   public List<DbMemberInterfaceImplementation> MemberInterfaceImplementations { get; set; } = [];
   
   [InverseProperty(nameof(DbMemberInterfaceImplementation.InterfaceSymbol))]
   public List<DbMemberInterfaceImplementation> ImplementedByMembers { get; set; } = [];
   
   [InverseProperty(nameof(DbParameterSymbol.Symbol))]
   public List<DbParameterSymbol> ParameterSymbols { get; set; } = [];
   
   [InverseProperty(nameof(ContainingTypeSymbol))]
   public List<DbSymbol> MemberSymbols { get; set; } = [];
   
   [InverseProperty(nameof(DbTypeInterface.TypeSymbol))]
   public List<DbTypeInterface> TypeInterfaces { get; set; } = [];
   
   [InverseProperty(nameof(DbTypeInterface.InterfaceSymbol))]
   public List<DbTypeInterface> ImplementedByTypes { get; set; } = [];
}