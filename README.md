# Beskar.CodeAnalytics

> **Caution**:
> **Work in Progress**
> This project is currently under active development. APIs and internal data structures are subject to change without notice.

## About the Project
**Beskar.CodeAnalytics** is a high-performance engine designed to collect, process, and analyze C# source code using the Roslyn compiler platform. It transforms complex codebases into a queryable, binary format optimized for high-speed analysis and low-memory environments.

### Technical Overview
* **Collection**: Utilizes `MSBuildWorkspace` and Roslyn's semantic models to discover solutions, projects, and symbols with deep semantic understanding.
* **Baking Engine**: A multi-step pipeline that sorts symbols, connects relational edges (e.g., base types, overrides), and generates specialized search indexes.
* **Indexing**: Implements custom `NGram` and `StaticWideBTree` indexes to enable lightning-fast symbol and text searching with a minimal memory footprint.
* **Storage**: Data is persisted in memory-mapped files (`.mmb`) using compact, sequential struct layouts with 1-byte packing to ensure zero-overhead, "instant-on" access.

---

## Solution Structure
The project is organized into three primary layers to separate concerns between data processing, core engine logic, and visualization:

* **Core**:
   * `Beskar.CodeAnalytics.Collector`: The logic for traversing Roslyn workspaces and extracting symbol data.
   * `Beskar.CodeAnalytics.Data`: The backbone of the engine, containing the baking logic, indexing structures, and MMF storage implementations.
* **Web**:
   * `Beskar.CodeAnalytics.Dashboard`: A Blazor-based UI for visualizing the analyzed code analytics.
   * `Beskar.CodeAnalytics.Dashboard.Shared`: Shared models and interfaces for the dashboard components.
* **Console**:
   * `Beskar.CodeAnalytics.Collector.Console`: A CLI tool to trigger the collection and baking process.
   * `Beskar.CodeAnalytics.Experiments`: A playground for testing new indexing and analysis algorithms.

---

## Memory Layout: Specification Structs

All specification structs use `[StructLayout(LayoutKind.Sequential, Pack = 1)]` to ensure a predictable and minimal binary footprint.

### Core Symbol Specifications

#### SymbolSpec (51 Bytes)
The base specification for all code symbols.
| Field | Type | Size | Description |
| :--- | :--- | :--- | :--- |
| **Id** | `uint` | 4 | Unique identifier for the symbol |
| **ContainingId** | `uint` | 4 | ID of the containing symbol |
| **Type** | `SymbolType` | 1 | Enum indicating the kind of symbol |
| **Accessibility** | `AccessModifier` | 1 | Visibility (Public, Private, etc.) |
| **Flags8** | `SymbolFlags` | 1 | **Bitfield** (Static, Abstract, Virtual, etc.) |
| **Name** | `StringFileView` | 8 | Reference to the symbol name string |
| **MetadataName** | `StringFileView` | 8 | Reference to the metadata name string |
| **FullPath** | `StringFileView` | 8 | Reference to the full symbol path string |
| **Declarations** | `StorageView` | 8 | View into associated declarations |
| **Locations** | `StorageView` | 8 | View into symbol locations |

#### TypeSymbolSpec (31 Bytes)
| Field | Type | Size | Description |
| :--- | :--- | :--- | :--- |
| **SymbolId** | `uint` | 4 | Link to the base SymbolSpec |
| **BaseTypeId** | `uint` | 4 | ID of the base type symbol |
| **TypeStorageKind** | `TypeStorageKind` | 1 | Enum for storage kind (Class, Struct, etc.) |
| **SpecialTypeKind** | `SpecialTypeKind` | 1 | Enum for Roslyn special types |
| **Flags8** | `TypeFlags` | 1 | **Bitfield** (IsReadOnly, IsRecord, etc.) |
| **AllInterfaces** | `StorageView` | 8 | View into all implemented interfaces |
| **DirectInterfaces** | `StorageView` | 8 | View into direct interfaces only |

#### MethodSymbolSpec (30 Bytes)
| Field | Type | Size | Description |
| :--- | :--- | :--- | :--- |
| **SymbolId** | `uint` | 4 | Link to the base SymbolSpec |
| **ReturnTypeId** | `uint` | 4 | ID of the return type symbol |
| **OverriddenMethodId** | `uint` | 4 | ID of the method this overrides |
| **Flags8** | `MethodFlags` | 1 | **Bitfield** (Async, ReadOnly, etc.) |
| **MethodType** | `MethodType` | 1 | Enum for method kind (Normal, Ctor, etc.) |
| **Parameters** | `StorageView` | 8 | View into the parameter list |
| **TypeParameters** | `StorageView` | 8 | View into type parameters |

#### PropertySymbolSpec (18 Bytes)
| Field | Type | Size | Description |
| :--- | :--- | :--- | :--- |
| **SymbolId** | `uint` | 4 | Link to the base SymbolSpec |
| **TypeId** | `uint` | 4 | ID of the property type symbol |
| **GetMethodId** | `uint` | 4 | ID of the getter method |
| **SetMethodId** | `uint` | 4 | ID of the setter method |
| **RefType** | `RefType` | 1 | Reference kind (Ref, Out, In, None) |
| **Flags8** | `PropertyFlags` | 1 | **Bitfield** (IsIndexer, IsReadOnly, etc.) |

#### FieldSymbolSpec (10 Bytes)
| Field | Type | Size | Description |
| :--- | :--- | :--- | :--- |
| **SymbolId** | `uint` | 4 | Link to the base SymbolSpec |
| **TypeId** | `uint` | 4 | ID of the field type symbol |
| **RefType** | `RefType` | 1 | Reference kind |
| **Flags8** | `FieldFlags` | 1 | **Bitfield** (IsReadOnly, IsVolatile, etc.) |

#### ParameterSymbolSpec (15 Bytes)
| Field | Type | Size | Description |
| :--- | :--- | :--- | :--- |
| **SymbolId** | `uint` | 4 | Link to the base SymbolSpec |
| **TypeId** | `uint` | 4 | ID of the parameter type symbol |
| **Ordinal** | `int` | 4 | Position in the parameter list |
| **ScopeType** | `ScopeType` | 1 | Scope kind (None, Scoped) |
| **RefType** | `RefType` | 1 | Reference kind |
| **Flags8** | `ParameterFlags` | 1 | **Bitfield** (IsOptional, IsParams, etc.) |

---

### Project & File Structure

#### ProjectSpec (56 Bytes)
| Field | Type | Size | Description |
| :--- | :--- | :--- | :--- |
| **Id** | `uint` | 4 | Unique identifier for the project |
| **Name** | `StringFileView` | 8 | Reference to project name string |
| **AssemblyName** | `StringFileView` | 8 | Reference to assembly name string |
| **FilePath** | `StringFileView` | 8 | Reference to project file path |
| **DefaultNamespace** | `StringFileView` | 8 | Reference to default namespace |
| **References** | `StorageView` | 8 | View into project references |
| **SolutionId** | `uint` | 4 | ID of the containing solution |
| **Files** | `StorageView` | 8 | View into project files |

#### FileSpec (28 Bytes)
| Field | Type | Size | Description |
| :--- | :--- | :--- | :--- |
| **Id** | `uint` | 4 | Unique identifier for the file |
| **FullPath** | `StringFileView` | 8 | Reference to full file path string |
| **Symbols** | `StorageView` | 8 | View into all symbols in this file |
| **Declarations** | `StorageView` | 8 | View into declarations in this file |

---

### Syntax & Tokens

#### SyntaxTokenSpec (15 Bytes)
Represents a single token within a source file for syntax highlighting and symbol mapping.
| Field | Type | Size | Description |
| :--- | :--- | :--- | :--- |
| **Start** | `int` | 4 | Character start position |
| **Length** | `int` | 4 | Length of the token |
| **Color** | `SyntaxColor` | 2 | Enum for syntax highlighting |
| **Flags8** | `SyntaxTokenFlags` | 1 | **Bitfield** (IsKeyword, IsIdentifier, etc.) |
| **SymbolId** | `uint` | 4 | ID of the associated symbol |
---

## Getting Started
1. **Prerequisites**: Ensure you have the .NET 10.0 SDK or later installed.
2. **Clone**: `git clone https://github.com/marvindrude/codeanalytics.engine.git`
3. **Build**: Run `dotnet build` from the root directory.
4. **Collect**: Use the `Collector.Console` to point the engine at a `.sln` or `.slnx` file to begin the baking process.

## License
See `LICENSE.md` for more information.
