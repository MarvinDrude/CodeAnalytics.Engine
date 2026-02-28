using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;

namespace Beskar.CodeAnalytics.Data.Entities.Structure;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SolutionSpec : ISpec
{
   public uint Identifier => Id;
   
   public uint Id;
   public StringFileView Name;
   public StringFileView FilePath;

   public StorageView<uint> Projects;

   public static FileId FileId => FileIds.Solution;
}