# Beskar.CodeAnalytics

> **Caution**:
> **Work in Progress**
> This project is currently under active development. APIs and internal data structures are subject to change without notice.

## About the Project
**Beskar.CodeAnalytics** is a high-performance engine designed to collect, process, and analyze C# source code using the Roslyn compiler platform. It transforms complex codebases into a queryable, binary format optimized for low-memory environments.

### Technical Overview
* **Collection**: Utilizes `MSBuildWorkspace` and Roslyn's semantic models to discover solutions, projects, and symbols.
* **Baking Engine**: A multi-step pipeline that sorts symbols, connects edges (relationships), and generates specialized indexes.
* **Indexing**: Implements custom `NGram` and `StaticWideBTree` indexes for fast searching while maintaining a small memory footprint.
* **Storage**: Data is stored in memory-mapped files (`.mmb`) using compact, sequential struct layouts with 1-byte packing to ensure zero-overhead access.

---

## Memory Layout: Specification Structs
All specification structs use `[StructLayout(LayoutKind.Sequential, Pack = 1)]` to ensure a predictable and minimal binary footprint.

## Flag Bitfield Visualization
To minimize memory usage, boolean properties are packed into single-byte bitfields (e.g., `Flags8`). Below is the mapping for these bytes:

### SymbolFlags (1 Byte)
| Bit | Property | Description |
| :--- | :--- | :--- |
| `0000 0001` | **IsStatic** | Symbol is declared with the static keyword |
| `0000 0010` | **IsAbstract** | Symbol is abstract |
| `0000 0100` | **IsVirtual** | Symbol is virtual |
| `0000 1000` | **IsOverride** | Symbol overrides a base member |
| `0001 0000` | **IsSealed** | Symbol is sealed |
| `0010 0000` | **IsExtern** | Symbol is defined externally |

---

## Memory Layout: Specification Structs
All specification structs use `[StructLayout(LayoutKind.Sequential, Pack = 1)]`.

### SymbolSpec
| Field | Type | Size (Bytes) | Description |
| :--- | :--- | :--- | :--- |
| **Id** | `uint` | 4 | Unique identifier for the symbol |
| **ContainingId** | `uint` | 4 | ID of the containing symbol |
| **Type** | `SymbolType` | 1 | Enum indicating the kind of symbol |
| **Accessibility** | `AccessModifier` | 1 | Visibility (Public, Private, etc.) |
| **Flags8** | `SymbolFlags` | 1 | **Bitfield** (See SymbolFlags table above) |
| **Name** | `StringFileView` | 8 | Reference to the symbol name string |
| **MetadataName** | `StringFileView` | 8 | Reference to the metadata name string |
| **FullPath** | `StringFileView` | 8 | Reference to the full symbol path string |
| **Declarations** | `StorageView` | 8 | View into associated declarations |
| **Locations** | `StorageView` | 8 | View into symbol locations |
| **Total** | | **51** | |

### MethodSymbolSpec
| Field | Type | Size (Bytes) | Description |
| :--- | :--- | :--- | :--- |
| **SymbolId** | `uint` | 4 | Link to the base SymbolSpec |
| **ReturnTypeId** | `uint` | 4 | ID of the return type symbol |
| **OverriddenMethodId** | `uint` | 4 | ID of the method this overrides |
| **Flags8** | `MethodFlags` | 1 | **Bitfield** (Async, ReadOnly, etc.) |
| **MethodType** | `MethodType` | 1 | Enum for method kind (Normal, Ctor, etc.) |
| **Parameters** | `StorageView` | 8 | View into the parameter list |
| **TypeParameters** | `StorageView` | 8 | View into type parameters |
| **Total** | | **30** | |

### TypeSymbolSpec
| Field | Type | Size (Bytes) | Description |
| :--- | :--- | :--- | :--- |
| **SymbolId** | `uint` | 4 | Link to the base SymbolSpec |
| **BaseTypeId** | `uint` | 4 | ID of the base type symbol |
| **TypeStorageKind** | `TypeStorageKind` | 1 | Enum for storage kind (Class, Struct, etc.) |
| **SpecialTypeKind** | `SpecialTypeKind` | 1 | Enum for Roslyn special types |
| **Flags8** | `TypeFlags` | 1 | **Bitfield** (IsReadOnly, IsRecord, etc.) |
| **AllInterfaces** | `StorageView` | 8 | View into all implemented interfaces |
| **DirectInterfaces** | `StorageView` | 8 | View into direct interfaces only |
| **Total** | | **31** | |

### FieldSymbolSpec
| Field | Type | Size (Bytes) | Description |
| :--- | :--- | :--- | :--- |
| **SymbolId** | `uint` | 4 | Link to the base SymbolSpec |
| **TypeId** | `uint` | 4 | ID of the field type symbol |
| **RefType** | `RefType` | 1 | Reference kind |
| **Flags8** | `FieldFlags` | 1 | **Bitfield** (IsReadOnly, IsVolatile, etc.) |
| **Total** | | **10** | |

### PropertySymbolSpec
| Field | Type | Size (Bytes) | Description |
| :--- | :--- | :--- | :--- |
| **SymbolId** | `uint` | 4 | Link to the base SymbolSpec |
| **TypeId** | `uint` | 4 | ID of the property type symbol |
| **GetMethodId** | `uint` | 4 | ID of the getter method |
| **SetMethodId** | `uint` | 4 | ID of the setter method |
| **RefType** | `RefType` | 1 | Reference kind (Ref, Out, In, None) |
| **Flags8** | `PropertyFlags` | 1 | **Bitfield** (IsIndexer, IsReadOnly, etc.) |
| **Total** | | **18** | |

### ParameterSymbolSpec
| Field | Type | Size (Bytes) | Description |
| :--- | :--- | :--- | :--- |
| **SymbolId** | `uint` | 4 | Link to the base SymbolSpec |
| **TypeId** | `uint` | 4 | ID of the parameter type symbol |
| **Ordinal** | `int` | 4 | Position in the parameter list |
| **ScopeType** | `ScopeType` | 1 | Scope kind (None, Scoped) |
| **RefType** | `RefType` | 1 | Reference kind |
| **Flags8** | `ParameterFlags` | 1 | **Bitfield** (IsOptional, IsParams, etc.) |
| **Total** | | **15** | |

### ProjectSpec
| Field | Type | Size (Bytes) | Description |
| :--- | :--- | :--- | :--- |
| **Id** | `uint` | 4 | Unique identifier for the project |
| **Name** | `StringFileView` | 8 | Reference to project name string |
| **AssemblyName** | `StringFileView` | 8 | Reference to assembly name string |
| **FilePath** | `StringFileView` | 8 | Reference to project file path |
| **DefaultNamespace** | `StringFileView` | 8 | Reference to default namespace |
| **References** | `StorageView` | 8 | View into project references |
| **SolutionId** | `uint` | 4 | ID of the containing solution |
| **Files** | `StorageView` | 8 | View into project files |
| **Total** | | **56** | |

### FileSpec
| Field | Type | Size (Bytes) | Description |
| :--- | :--- | :--- | :--- |
| **Id** | `uint` | 4 | Unique identifier for the file |
| **FullPath** | `StringFileView` | 8 | Reference to full file path string |
| **Symbols** | `StorageView` | 8 | View into all symbols in this file |
| **Declarations** | `StorageView` | 8 | View into declarations in this file |
| **Total** | | **28** | |

### SyntaxTokenSpec
| Field | Type | Size (Bytes) | Description |
| :--- | :--- | :--- | :--- |
| **Start** | `int` | 4 | Character start position |
| **Length** | `int` | 4 | Length of the token |
| **Color** | `SyntaxColor` | 2 | Enum for syntax highlighting |
| **Flags8** | `SyntaxTokenFlags` | 1 | **Bitfield** (IsKeyword, IsIdentifier, etc.) |
| **SymbolId** | `uint` | 4 | ID of the associated symbol |
| **Total** | | **15** | |