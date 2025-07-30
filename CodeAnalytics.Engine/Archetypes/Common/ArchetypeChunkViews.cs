using CodeAnalytics.Engine.Analyze;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;

namespace CodeAnalytics.Engine.Archetypes.Common;

public sealed class ArchetypeChunkViews
{
   public ArchetypeChunkView<ClassArchetype> Classes { get; }
   public ArchetypeChunkView<StructArchetype> Structs { get; }
   public ArchetypeChunkView<InterfaceArchetype> Interfaces { get; }
   public ArchetypeChunkView<EnumArchetype> Enums { get; }
   public ArchetypeChunkView<EnumValueArchetype> EnumValues { get; }
   
   public ArchetypeChunkView<ConstructorArchetype> Constructors { get; }
   public ArchetypeChunkView<MethodArchetype> Methods { get; }
   public ArchetypeChunkView<PropertyArchetype> Properties { get; }
   public ArchetypeChunkView<FieldArchetype> Fields { get; }
   
   public ArchetypeChunkViews(AnalyzeStore store)
   {
      Classes = new ArchetypeChunkView<ClassArchetype>(store.ClassChunk);
      Structs = new ArchetypeChunkView<StructArchetype>(store.StructChunk);
      Interfaces = new ArchetypeChunkView<InterfaceArchetype>(store.InterfaceChunk);
      Enums = new ArchetypeChunkView<EnumArchetype>(store.EnumChunk);
      EnumValues = new ArchetypeChunkView<EnumValueArchetype>(store.EnumValueChunk);
      
      Constructors = new ArchetypeChunkView<ConstructorArchetype>(store.ConstructorChunk);
      Methods = new ArchetypeChunkView<MethodArchetype>(store.MethodChunk);
      Properties = new ArchetypeChunkView<PropertyArchetype>(store.PropertyChunk);
      Fields = new ArchetypeChunkView<FieldArchetype>(store.FieldChunk);
   }
}