using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Me.Memory.Buffers.Dynamic;

namespace Beskar.CodeAnalytics.Data.Entities.Structure;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FolderSpec : ISpec
{
   public uint Id;
   public uint ParentId;
   
   public StringFileView Name;
   public StringFileView FullPath;

   public PackedBools8 Flags;

   public StorageView<FolderSpec> SubFolders;
   public StorageView<FileSpec> Files;
   
   public StorageView<uint> Projects;
   
   public uint Identifier => Id;
   public static FileId FileId => FileIds.Folder;

   public bool IsVirtual
   {
      get => Flags.Get(0);
      set => Flags.Set(0, value);
   }

   public bool IsRoot
   {
      get => Flags.Get(1);
      set => Flags.Set(1, value);
   }
   
   public static readonly IComparer<FolderSpec> Comparer = Comparer<FolderSpec>.Create(static (x, y) => x.Id.CompareTo(y.Id));
}