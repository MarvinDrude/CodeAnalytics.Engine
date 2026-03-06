using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Data.Metadata.Models;
using Beskar.CodeAnalytics.Data.Metadata.Specs;
using Me.Memory.Buffers;
using Me.Memory.Serialization;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Descriptors;

public sealed class StructureDescriptorSerializer : ISerializer<StructureDescriptor>
{
   private readonly ISerializer<FolderSpecDescriptor> _folderSpecSerializer = SerializerRegistry.For<FolderSpecDescriptor>();
   private readonly ISerializer<FileSpecDescriptor> _fileSpecSerializer = SerializerRegistry.For<FileSpecDescriptor>();
   private readonly ISerializer<ProjectSpecDescriptor> _projectSpecSerializer = SerializerRegistry.For<ProjectSpecDescriptor>();
   private readonly ISerializer<SolutionSpecDescriptor> _solutionSpecSerializer = SerializerRegistry.For<SolutionSpecDescriptor>();
   private readonly ISerializer<SyntaxFileDescriptor> _syntaxFileSerializer = SerializerRegistry.For<SyntaxFileDescriptor>();
   private readonly ISerializer<SymbolLocationSpecDescriptor> _symbolLocationSerializer = SerializerRegistry.For<SymbolLocationSpecDescriptor>();
   
   public void Write(ref ByteWriter writer, ref StructureDescriptor value)
   {
      var folders = value.Folders;
      _folderSpecSerializer.Write(ref writer, ref folders);
      
      var files = value.Files;
      _fileSpecSerializer.Write(ref writer, ref files);
      
      var projects = value.Projects;
      _projectSpecSerializer.Write(ref writer, ref projects);
      
      var solutions = value.Solutions;
      _solutionSpecSerializer.Write(ref writer, ref solutions);
      
      var syntaxFiles = value.SyntaxFiles;
      _syntaxFileSerializer.Write(ref writer, ref syntaxFiles);
      
      var symbolLocations = value.SymbolLocations;
      _symbolLocationSerializer.Write(ref writer, ref symbolLocations);
      
      writer.WriteLittleEndian(value.RootFolderId);
   }

   public bool TryRead(ref ByteReader reader, [MaybeNullWhen(false)] out StructureDescriptor value)
   {
      value = null;

      if (!_folderSpecSerializer.TryRead(ref reader, out var folders)
          || !_fileSpecSerializer.TryRead(ref reader, out var files)
          || !_projectSpecSerializer.TryRead(ref reader, out var projects)
          || !_solutionSpecSerializer.TryRead(ref reader, out var solutions)
          || !_syntaxFileSerializer.TryRead(ref reader, out var syntaxFiles)
          || !_symbolLocationSerializer.TryRead(ref reader, out var symbolLocations))
      {
         return false;
      }

      var rootFolderId = reader.ReadLittleEndian<uint>();

      value = new StructureDescriptor()
      {
         Folders = folders,
         Files = files,
         Projects = projects,
         Solutions = solutions,
         SyntaxFiles = syntaxFiles,
         SymbolLocations = symbolLocations,
         RootFolderId = rootFolderId
      };
      
      return true;
   }

   public int CalculateByteLength(ref StructureDescriptor value)
   {
      var folders = value.Folders;
      var files = value.Files;
      var projects = value.Projects;
      var solutions = value.Solutions;
      var syntaxFiles = value.SyntaxFiles;
      var symbolLocations = value.SymbolLocations;
      
      return _folderSpecSerializer.CalculateByteLength(ref folders) 
             + _fileSpecSerializer.CalculateByteLength(ref files) 
             + _projectSpecSerializer.CalculateByteLength(ref projects) 
             + _solutionSpecSerializer.CalculateByteLength(ref solutions) 
             + _syntaxFileSerializer.CalculateByteLength(ref syntaxFiles) 
             + _symbolLocationSerializer.CalculateByteLength(ref symbolLocations) 
             + sizeof(uint);
   }
}