using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;

namespace Beskar.CodeAnalytics.Data.Entities.Structure;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ProjectSpec : ISpec
{
   public uint Id;
   
   public StringFileView Name;
   public StringFileView AssemblyName;
   public StringFileView ProjectFilePath;
   
   public StorageView<ProjectSpec> ProjectReferences;
   public StorageView<SolutionSpec> Solutions;
   public StorageView<FileSpec> Files;

   public uint Identifier => Id;
   public static FileId FileId => FileIds.Project;
}