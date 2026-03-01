using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;

namespace Beskar.CodeAnalytics.Data.Entities.Structure;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FolderSpec : ISpec
{
   public uint Id;
   public uint ParentId;
   
   public StringFileView Name;
   public StringFileView FullPath;

   public Flags8 Flags;

   public StorageView<FolderSpec> SubFolders;
   public StorageView<FileSpec> Files;
   
   public StorageView<uint> Projects;
   
   public uint Identifier => Id;
   public static FileId FileId => FileIds.Folder;

   public bool IsVirtual
   {
      get => Flags[0].Get(0);
      set => Flags[0].Set(0, value);
   }

   public bool IsRoot
   {
      get => Flags[0].Get(1);
      set => Flags[0].Set(1, value);
   }
}