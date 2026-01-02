using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CodeAnalytics.Engine.Storage.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    RelativeFilePath = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    RelativeFilePath = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    AssemblyName = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Solutions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    RelativeFilePath = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Symbols",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UniqueId = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    UniqueIdHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    MetadataName = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    FullPathName = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    Language = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    IsVirtual = table.Column<bool>(type: "boolean", nullable: false),
                    IsAbstract = table.Column<bool>(type: "boolean", nullable: false),
                    IsStatic = table.Column<bool>(type: "boolean", nullable: false),
                    IsSealed = table.Column<bool>(type: "boolean", nullable: false),
                    IsGenerated = table.Column<bool>(type: "boolean", nullable: false),
                    AccessModifier = table.Column<byte>(type: "smallint", nullable: false),
                    Type = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symbols", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DbFileDbProject",
                columns: table => new
                {
                    FilesId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbFileDbProject", x => new { x.FilesId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_DbFileDbProject_Files_FilesId",
                        column: x => x.FilesId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbFileDbProject_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbProjectDbProject",
                columns: table => new
                {
                    ReferencedByProjectsId = table.Column<long>(type: "bigint", nullable: false),
                    ReferencedProjectsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbProjectDbProject", x => new { x.ReferencedByProjectsId, x.ReferencedProjectsId });
                    table.ForeignKey(
                        name: "FK_DbProjectDbProject_Projects_ReferencedByProjectsId",
                        column: x => x.ReferencedByProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbProjectDbProject_Projects_ReferencedProjectsId",
                        column: x => x.ReferencedProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbProjectDbSolution",
                columns: table => new
                {
                    ProjectsId = table.Column<long>(type: "bigint", nullable: false),
                    SolutionsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbProjectDbSolution", x => new { x.ProjectsId, x.SolutionsId });
                    table.ForeignKey(
                        name: "FK_DbProjectDbSolution_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbProjectDbSolution_Solutions_SolutionsId",
                        column: x => x.SolutionsId,
                        principalTable: "Solutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassSymbols",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsRecord = table.Column<bool>(type: "boolean", nullable: false),
                    BaseClassSymbolId = table.Column<long>(type: "bigint", nullable: true),
                    SymbolId = table.Column<long>(type: "bigint", nullable: false),
                    IsAnonymous = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSymbols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassSymbols_Symbols_BaseClassSymbolId",
                        column: x => x.BaseClassSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSymbols_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnumSymbols",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UnderlyingTypeSymbolId = table.Column<long>(type: "bigint", nullable: false),
                    SymbolId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnumSymbols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnumSymbols_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnumSymbols_Symbols_UnderlyingTypeSymbolId",
                        column: x => x.UnderlyingTypeSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldSymbols",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsConst = table.Column<bool>(type: "boolean", nullable: false),
                    IsReadOnly = table.Column<bool>(type: "boolean", nullable: false),
                    IsVolatile = table.Column<bool>(type: "boolean", nullable: false),
                    Nullability = table.Column<byte>(type: "smallint", nullable: false),
                    TypeSymbolId = table.Column<long>(type: "bigint", nullable: false),
                    SymbolId = table.Column<long>(type: "bigint", nullable: false),
                    ContainingSymbolId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldSymbols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldSymbols_Symbols_ContainingSymbolId",
                        column: x => x.ContainingSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldSymbols_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldSymbols_Symbols_TypeSymbolId",
                        column: x => x.TypeSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterfaceSymbols",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SymbolId = table.Column<long>(type: "bigint", nullable: false),
                    IsAnonymous = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterfaceSymbols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterfaceSymbols_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MethodSymbols",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsAsync = table.Column<bool>(type: "boolean", nullable: false),
                    IsConstructor = table.Column<bool>(type: "boolean", nullable: false),
                    IsGeneric = table.Column<bool>(type: "boolean", nullable: false),
                    CyclomaticComplexity = table.Column<int>(type: "integer", nullable: false),
                    ReturnTypeNullability = table.Column<byte>(type: "smallint", nullable: false),
                    ReturnTypeSymbolId = table.Column<long>(type: "bigint", nullable: false),
                    OverriddenSymbolId = table.Column<long>(type: "bigint", nullable: true),
                    SymbolId = table.Column<long>(type: "bigint", nullable: false),
                    ContainingSymbolId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodSymbols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MethodSymbols_Symbols_ContainingSymbolId",
                        column: x => x.ContainingSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MethodSymbols_Symbols_OverriddenSymbolId",
                        column: x => x.OverriddenSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MethodSymbols_Symbols_ReturnTypeSymbolId",
                        column: x => x.ReturnTypeSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MethodSymbols_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParameterSymbols",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Modifiers = table.Column<int>(type: "integer", nullable: false),
                    Nullability = table.Column<byte>(type: "smallint", nullable: false),
                    Ordinal = table.Column<int>(type: "integer", nullable: false),
                    IsParams = table.Column<bool>(type: "boolean", nullable: false),
                    IsThis = table.Column<bool>(type: "boolean", nullable: false),
                    IsOptional = table.Column<bool>(type: "boolean", nullable: false),
                    IsDiscard = table.Column<bool>(type: "boolean", nullable: false),
                    HasExplicitDefaultValue = table.Column<bool>(type: "boolean", nullable: false),
                    TypeSymbolId = table.Column<long>(type: "bigint", nullable: false),
                    SymbolId = table.Column<long>(type: "bigint", nullable: false),
                    ContainingSymbolId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterSymbols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParameterSymbols_Symbols_ContainingSymbolId",
                        column: x => x.ContainingSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParameterSymbols_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParameterSymbols_Symbols_TypeSymbolId",
                        column: x => x.TypeSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertySymbols",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nullability = table.Column<byte>(type: "smallint", nullable: false),
                    ReturnsByRefReadonly = table.Column<bool>(type: "boolean", nullable: false),
                    ReturnsByRef = table.Column<bool>(type: "boolean", nullable: false),
                    TypeSymbolId = table.Column<long>(type: "bigint", nullable: false),
                    GetterSymbolId = table.Column<long>(type: "bigint", nullable: true),
                    SetterSymbolId = table.Column<long>(type: "bigint", nullable: true),
                    OverriddenSymbolId = table.Column<long>(type: "bigint", nullable: true),
                    SymbolId = table.Column<long>(type: "bigint", nullable: false),
                    ContainingSymbolId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertySymbols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertySymbols_Symbols_ContainingSymbolId",
                        column: x => x.ContainingSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertySymbols_Symbols_GetterSymbolId",
                        column: x => x.GetterSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertySymbols_Symbols_OverriddenSymbolId",
                        column: x => x.OverriddenSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertySymbols_Symbols_SetterSymbolId",
                        column: x => x.SetterSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertySymbols_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertySymbols_Symbols_TypeSymbolId",
                        column: x => x.TypeSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StructSymbols",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsRecord = table.Column<bool>(type: "boolean", nullable: false),
                    IsReadOnly = table.Column<bool>(type: "boolean", nullable: false),
                    IsRef = table.Column<bool>(type: "boolean", nullable: false),
                    SymbolId = table.Column<long>(type: "bigint", nullable: false),
                    IsAnonymous = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StructSymbols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StructSymbols_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SymbolReferences",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SymbolId = table.Column<long>(type: "bigint", nullable: false),
                    FileId = table.Column<long>(type: "bigint", nullable: false),
                    SpanIndex = table.Column<int>(type: "integer", nullable: false),
                    IsDefinition = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymbolReferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymbolReferences_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SymbolReferences_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbClassSymbolDbInterfaceSymbol",
                columns: table => new
                {
                    ImplementedByClassId = table.Column<long>(type: "bigint", nullable: false),
                    ImplementedInterfacesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbClassSymbolDbInterfaceSymbol", x => new { x.ImplementedByClassId, x.ImplementedInterfacesId });
                    table.ForeignKey(
                        name: "FK_DbClassSymbolDbInterfaceSymbol_ClassSymbols_ImplementedByCl~",
                        column: x => x.ImplementedByClassId,
                        principalTable: "ClassSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbClassSymbolDbInterfaceSymbol_InterfaceSymbols_Implemented~",
                        column: x => x.ImplementedInterfacesId,
                        principalTable: "InterfaceSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbClassSymbolDbInterfaceSymbol1",
                columns: table => new
                {
                    ImplementedDirectByClassId = table.Column<long>(type: "bigint", nullable: false),
                    ImplementedDirectInterfacesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbClassSymbolDbInterfaceSymbol1", x => new { x.ImplementedDirectByClassId, x.ImplementedDirectInterfacesId });
                    table.ForeignKey(
                        name: "FK_DbClassSymbolDbInterfaceSymbol1_ClassSymbols_ImplementedDir~",
                        column: x => x.ImplementedDirectByClassId,
                        principalTable: "ClassSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbClassSymbolDbInterfaceSymbol1_InterfaceSymbols_Implemente~",
                        column: x => x.ImplementedDirectInterfacesId,
                        principalTable: "InterfaceSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbInterfaceSymbolDbInterfaceSymbol",
                columns: table => new
                {
                    ImplementedByInterfaceId = table.Column<long>(type: "bigint", nullable: false),
                    ImplementedInterfacesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbInterfaceSymbolDbInterfaceSymbol", x => new { x.ImplementedByInterfaceId, x.ImplementedInterfacesId });
                    table.ForeignKey(
                        name: "FK_DbInterfaceSymbolDbInterfaceSymbol_InterfaceSymbols_Impleme~",
                        column: x => x.ImplementedByInterfaceId,
                        principalTable: "InterfaceSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbInterfaceSymbolDbInterfaceSymbol_InterfaceSymbols_Implem~1",
                        column: x => x.ImplementedInterfacesId,
                        principalTable: "InterfaceSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbInterfaceSymbolDbInterfaceSymbol1",
                columns: table => new
                {
                    ImplementedDirectByInterfaceId = table.Column<long>(type: "bigint", nullable: false),
                    ImplementedDirectInterfacesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbInterfaceSymbolDbInterfaceSymbol1", x => new { x.ImplementedDirectByInterfaceId, x.ImplementedDirectInterfacesId });
                    table.ForeignKey(
                        name: "FK_DbInterfaceSymbolDbInterfaceSymbol1_InterfaceSymbols_Implem~",
                        column: x => x.ImplementedDirectByInterfaceId,
                        principalTable: "InterfaceSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbInterfaceSymbolDbInterfaceSymbol1_InterfaceSymbols_Imple~1",
                        column: x => x.ImplementedDirectInterfacesId,
                        principalTable: "InterfaceSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbInterfaceSymbolDbStructSymbol",
                columns: table => new
                {
                    ImplementedByStructId = table.Column<long>(type: "bigint", nullable: false),
                    ImplementedInterfacesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbInterfaceSymbolDbStructSymbol", x => new { x.ImplementedByStructId, x.ImplementedInterfacesId });
                    table.ForeignKey(
                        name: "FK_DbInterfaceSymbolDbStructSymbol_InterfaceSymbols_Implemente~",
                        column: x => x.ImplementedInterfacesId,
                        principalTable: "InterfaceSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbInterfaceSymbolDbStructSymbol_StructSymbols_ImplementedBy~",
                        column: x => x.ImplementedByStructId,
                        principalTable: "StructSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbInterfaceSymbolDbStructSymbol1",
                columns: table => new
                {
                    ImplementedDirectByStructId = table.Column<long>(type: "bigint", nullable: false),
                    ImplementedDirectInterfacesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbInterfaceSymbolDbStructSymbol1", x => new { x.ImplementedDirectByStructId, x.ImplementedDirectInterfacesId });
                    table.ForeignKey(
                        name: "FK_DbInterfaceSymbolDbStructSymbol1_InterfaceSymbols_Implement~",
                        column: x => x.ImplementedDirectInterfacesId,
                        principalTable: "InterfaceSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbInterfaceSymbolDbStructSymbol1_StructSymbols_ImplementedD~",
                        column: x => x.ImplementedDirectByStructId,
                        principalTable: "StructSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassSymbols_BaseClassSymbolId",
                table: "ClassSymbols",
                column: "BaseClassSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSymbols_SymbolId",
                table: "ClassSymbols",
                column: "SymbolId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DbClassSymbolDbInterfaceSymbol_ImplementedInterfacesId",
                table: "DbClassSymbolDbInterfaceSymbol",
                column: "ImplementedInterfacesId");

            migrationBuilder.CreateIndex(
                name: "IX_DbClassSymbolDbInterfaceSymbol1_ImplementedDirectInterfaces~",
                table: "DbClassSymbolDbInterfaceSymbol1",
                column: "ImplementedDirectInterfacesId");

            migrationBuilder.CreateIndex(
                name: "IX_DbFileDbProject_ProjectsId",
                table: "DbFileDbProject",
                column: "ProjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_DbInterfaceSymbolDbInterfaceSymbol_ImplementedInterfacesId",
                table: "DbInterfaceSymbolDbInterfaceSymbol",
                column: "ImplementedInterfacesId");

            migrationBuilder.CreateIndex(
                name: "IX_DbInterfaceSymbolDbInterfaceSymbol1_ImplementedDirectInterf~",
                table: "DbInterfaceSymbolDbInterfaceSymbol1",
                column: "ImplementedDirectInterfacesId");

            migrationBuilder.CreateIndex(
                name: "IX_DbInterfaceSymbolDbStructSymbol_ImplementedInterfacesId",
                table: "DbInterfaceSymbolDbStructSymbol",
                column: "ImplementedInterfacesId");

            migrationBuilder.CreateIndex(
                name: "IX_DbInterfaceSymbolDbStructSymbol1_ImplementedDirectInterface~",
                table: "DbInterfaceSymbolDbStructSymbol1",
                column: "ImplementedDirectInterfacesId");

            migrationBuilder.CreateIndex(
                name: "IX_DbProjectDbProject_ReferencedProjectsId",
                table: "DbProjectDbProject",
                column: "ReferencedProjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_DbProjectDbSolution_SolutionsId",
                table: "DbProjectDbSolution",
                column: "SolutionsId");

            migrationBuilder.CreateIndex(
                name: "IX_EnumSymbols_SymbolId",
                table: "EnumSymbols",
                column: "SymbolId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnumSymbols_UnderlyingTypeSymbolId",
                table: "EnumSymbols",
                column: "UnderlyingTypeSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldSymbols_ContainingSymbolId",
                table: "FieldSymbols",
                column: "ContainingSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldSymbols_SymbolId",
                table: "FieldSymbols",
                column: "SymbolId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FieldSymbols_TypeSymbolId",
                table: "FieldSymbols",
                column: "TypeSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_Name",
                table: "Files",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Files_RelativeFilePath",
                table: "Files",
                column: "RelativeFilePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterfaceSymbols_SymbolId",
                table: "InterfaceSymbols",
                column: "SymbolId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MethodSymbols_ContainingSymbolId",
                table: "MethodSymbols",
                column: "ContainingSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodSymbols_OverriddenSymbolId",
                table: "MethodSymbols",
                column: "OverriddenSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodSymbols_ReturnTypeSymbolId",
                table: "MethodSymbols",
                column: "ReturnTypeSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodSymbols_SymbolId",
                table: "MethodSymbols",
                column: "SymbolId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParameterSymbols_ContainingSymbolId",
                table: "ParameterSymbols",
                column: "ContainingSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterSymbols_SymbolId",
                table: "ParameterSymbols",
                column: "SymbolId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParameterSymbols_TypeSymbolId",
                table: "ParameterSymbols",
                column: "TypeSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AssemblyName",
                table: "Projects",
                column: "AssemblyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name",
                table: "Projects",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_RelativeFilePath",
                table: "Projects",
                column: "RelativeFilePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertySymbols_ContainingSymbolId",
                table: "PropertySymbols",
                column: "ContainingSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertySymbols_GetterSymbolId",
                table: "PropertySymbols",
                column: "GetterSymbolId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertySymbols_OverriddenSymbolId",
                table: "PropertySymbols",
                column: "OverriddenSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertySymbols_SetterSymbolId",
                table: "PropertySymbols",
                column: "SetterSymbolId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertySymbols_SymbolId",
                table: "PropertySymbols",
                column: "SymbolId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertySymbols_TypeSymbolId",
                table: "PropertySymbols",
                column: "TypeSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_Name",
                table: "Solutions",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_RelativeFilePath",
                table: "Solutions",
                column: "RelativeFilePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StructSymbols_SymbolId",
                table: "StructSymbols",
                column: "SymbolId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SymbolReferences_FileId",
                table: "SymbolReferences",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_SymbolReferences_SymbolId",
                table: "SymbolReferences",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_UniqueId",
                table: "Symbols",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_UniqueIdHash",
                table: "Symbols",
                column: "UniqueIdHash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbClassSymbolDbInterfaceSymbol");

            migrationBuilder.DropTable(
                name: "DbClassSymbolDbInterfaceSymbol1");

            migrationBuilder.DropTable(
                name: "DbFileDbProject");

            migrationBuilder.DropTable(
                name: "DbInterfaceSymbolDbInterfaceSymbol");

            migrationBuilder.DropTable(
                name: "DbInterfaceSymbolDbInterfaceSymbol1");

            migrationBuilder.DropTable(
                name: "DbInterfaceSymbolDbStructSymbol");

            migrationBuilder.DropTable(
                name: "DbInterfaceSymbolDbStructSymbol1");

            migrationBuilder.DropTable(
                name: "DbProjectDbProject");

            migrationBuilder.DropTable(
                name: "DbProjectDbSolution");

            migrationBuilder.DropTable(
                name: "EnumSymbols");

            migrationBuilder.DropTable(
                name: "FieldSymbols");

            migrationBuilder.DropTable(
                name: "MethodSymbols");

            migrationBuilder.DropTable(
                name: "ParameterSymbols");

            migrationBuilder.DropTable(
                name: "PropertySymbols");

            migrationBuilder.DropTable(
                name: "SymbolReferences");

            migrationBuilder.DropTable(
                name: "ClassSymbols");

            migrationBuilder.DropTable(
                name: "InterfaceSymbols");

            migrationBuilder.DropTable(
                name: "StructSymbols");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Solutions");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Symbols");
        }
    }
}
