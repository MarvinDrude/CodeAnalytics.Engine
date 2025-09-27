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
                    Name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    RelativeFilePath = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false)
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
                    Name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    RelativeFilePath = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    AssemblyName = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
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
                    Name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    RelativeFilePath = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false)
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
                    UniqueIdHash = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    MetadataName = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    FullPathName = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    Language = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    IsVirtual = table.Column<bool>(type: "boolean", nullable: false),
                    IsAbstract = table.Column<bool>(type: "boolean", nullable: false),
                    IsStatic = table.Column<bool>(type: "boolean", nullable: false),
                    IsSealed = table.Column<bool>(type: "boolean", nullable: false),
                    IsGenerated = table.Column<bool>(type: "boolean", nullable: false),
                    AccessModifier = table.Column<byte>(type: "smallint", nullable: false),
                    Type = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ContainingSymbolId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symbols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Symbols_Symbols_ContainingSymbolId",
                        column: x => x.ContainingSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "ProjectReferences",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SourceProjectId = table.Column<long>(type: "bigint", nullable: false),
                    ReferencedProjectId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectReferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectReferences_Projects_ReferencedProjectId",
                        column: x => x.ReferencedProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectReferences_Projects_SourceProjectId",
                        column: x => x.SourceProjectId,
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
                    BaseClassSymbolId = table.Column<long>(type: "bigint", nullable: false),
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
                    UnderlyingTypeId = table.Column<long>(type: "bigint", nullable: false),
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
                        name: "FK_EnumSymbols_Symbols_UnderlyingTypeId",
                        column: x => x.UnderlyingTypeId,
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
                name: "MemberInterfaceImplementations",
                columns: table => new
                {
                    SymbolId = table.Column<long>(type: "bigint", nullable: false),
                    InterfaceSymbolId = table.Column<long>(type: "bigint", nullable: false),
                    IsExplicit = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberInterfaceImplementations", x => new { x.SymbolId, x.InterfaceSymbolId });
                    table.ForeignKey(
                        name: "FK_MemberInterfaceImplementations_Symbols_InterfaceSymbolId",
                        column: x => x.InterfaceSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberInterfaceImplementations_Symbols_SymbolId",
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
                    ReturnTypeId = table.Column<long>(type: "bigint", nullable: false),
                    OverriddenSymbolId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodSymbols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MethodSymbols_Symbols_OverriddenSymbolId",
                        column: x => x.OverriddenSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MethodSymbols_Symbols_ReturnTypeId",
                        column: x => x.ReturnTypeId,
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
                    GetterCyclomaticComplexity = table.Column<int>(type: "integer", nullable: false),
                    SetterCylclomaticComplexity = table.Column<int>(type: "integer", nullable: false),
                    TypeSymbolId = table.Column<long>(type: "bigint", nullable: false),
                    GetterMethodId = table.Column<long>(type: "bigint", nullable: false),
                    SetterMethodId = table.Column<long>(type: "bigint", nullable: false),
                    OverriddenSymbolId = table.Column<long>(type: "bigint", nullable: false),
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
                        name: "FK_PropertySymbols_Symbols_GetterMethodId",
                        column: x => x.GetterMethodId,
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
                        name: "FK_PropertySymbols_Symbols_SetterMethodId",
                        column: x => x.SetterMethodId,
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
                name: "TypeInterfaces",
                columns: table => new
                {
                    TypeSymbolId = table.Column<long>(type: "bigint", nullable: false),
                    InterfaceSymbolId = table.Column<long>(type: "bigint", nullable: false),
                    IsDirect = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeInterfaces", x => new { x.TypeSymbolId, x.InterfaceSymbolId });
                    table.ForeignKey(
                        name: "FK_TypeInterfaces_Symbols_InterfaceSymbolId",
                        column: x => x.InterfaceSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TypeInterfaces_Symbols_TypeSymbolId",
                        column: x => x.TypeSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassSymbols_BaseClassSymbolId",
                table: "ClassSymbols",
                column: "BaseClassSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSymbols_Id",
                table: "ClassSymbols",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSymbols_SymbolId",
                table: "ClassSymbols",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_DbFileDbProject_ProjectsId",
                table: "DbFileDbProject",
                column: "ProjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_DbProjectDbSolution_SolutionsId",
                table: "DbProjectDbSolution",
                column: "SolutionsId");

            migrationBuilder.CreateIndex(
                name: "IX_EnumSymbols_Id",
                table: "EnumSymbols",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnumSymbols_SymbolId",
                table: "EnumSymbols",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_EnumSymbols_UnderlyingTypeId",
                table: "EnumSymbols",
                column: "UnderlyingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldSymbols_ContainingSymbolId",
                table: "FieldSymbols",
                column: "ContainingSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldSymbols_Id",
                table: "FieldSymbols",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FieldSymbols_SymbolId",
                table: "FieldSymbols",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldSymbols_TypeSymbolId",
                table: "FieldSymbols",
                column: "TypeSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_Id",
                table: "Files",
                column: "Id",
                unique: true);

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
                name: "IX_InterfaceSymbols_Id",
                table: "InterfaceSymbols",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterfaceSymbols_SymbolId",
                table: "InterfaceSymbols",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberInterfaceImplementations_InterfaceSymbolId",
                table: "MemberInterfaceImplementations",
                column: "InterfaceSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberInterfaceImplementations_SymbolId",
                table: "MemberInterfaceImplementations",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodSymbols_Id",
                table: "MethodSymbols",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MethodSymbols_OverriddenSymbolId",
                table: "MethodSymbols",
                column: "OverriddenSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodSymbols_ReturnTypeId",
                table: "MethodSymbols",
                column: "ReturnTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterSymbols_ContainingSymbolId",
                table: "ParameterSymbols",
                column: "ContainingSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterSymbols_Id",
                table: "ParameterSymbols",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParameterSymbols_SymbolId",
                table: "ParameterSymbols",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterSymbols_TypeSymbolId",
                table: "ParameterSymbols",
                column: "TypeSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReferences_Id",
                table: "ProjectReferences",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReferences_ReferencedProjectId",
                table: "ProjectReferences",
                column: "ReferencedProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReferences_SourceProjectId",
                table: "ProjectReferences",
                column: "SourceProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Id",
                table: "Projects",
                column: "Id",
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
                name: "IX_PropertySymbols_GetterMethodId",
                table: "PropertySymbols",
                column: "GetterMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertySymbols_Id",
                table: "PropertySymbols",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertySymbols_OverriddenSymbolId",
                table: "PropertySymbols",
                column: "OverriddenSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertySymbols_SetterMethodId",
                table: "PropertySymbols",
                column: "SetterMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertySymbols_SymbolId",
                table: "PropertySymbols",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertySymbols_TypeSymbolId",
                table: "PropertySymbols",
                column: "TypeSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_Id",
                table: "Solutions",
                column: "Id",
                unique: true);

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
                name: "IX_StructSymbols_Id",
                table: "StructSymbols",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StructSymbols_SymbolId",
                table: "StructSymbols",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_SymbolReferences_FileId",
                table: "SymbolReferences",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_SymbolReferences_Id",
                table: "SymbolReferences",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SymbolReferences_SymbolId",
                table: "SymbolReferences",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_ContainingSymbolId",
                table: "Symbols",
                column: "ContainingSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_Id",
                table: "Symbols",
                column: "Id",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_TypeInterfaces_InterfaceSymbolId",
                table: "TypeInterfaces",
                column: "InterfaceSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeInterfaces_TypeSymbolId",
                table: "TypeInterfaces",
                column: "TypeSymbolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassSymbols");

            migrationBuilder.DropTable(
                name: "DbFileDbProject");

            migrationBuilder.DropTable(
                name: "DbProjectDbSolution");

            migrationBuilder.DropTable(
                name: "EnumSymbols");

            migrationBuilder.DropTable(
                name: "FieldSymbols");

            migrationBuilder.DropTable(
                name: "InterfaceSymbols");

            migrationBuilder.DropTable(
                name: "MemberInterfaceImplementations");

            migrationBuilder.DropTable(
                name: "MethodSymbols");

            migrationBuilder.DropTable(
                name: "ParameterSymbols");

            migrationBuilder.DropTable(
                name: "ProjectReferences");

            migrationBuilder.DropTable(
                name: "PropertySymbols");

            migrationBuilder.DropTable(
                name: "StructSymbols");

            migrationBuilder.DropTable(
                name: "SymbolReferences");

            migrationBuilder.DropTable(
                name: "TypeInterfaces");

            migrationBuilder.DropTable(
                name: "Solutions");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Symbols");
        }
    }
}
