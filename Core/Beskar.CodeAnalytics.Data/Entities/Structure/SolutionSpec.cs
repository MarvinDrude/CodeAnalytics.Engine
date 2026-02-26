using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Misc;

namespace Beskar.CodeAnalytics.Data.Entities.Structure;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SolutionSpec
{
   public uint Id;
   public StringFileView Name;
   public StringFileView FilePath;

   public StorageView<ProjectSpec> Projects;

   public uint Identifier => Id;
   public static FileId FileId => FileIds.Solution;
}