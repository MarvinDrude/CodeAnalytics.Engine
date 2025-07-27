using CodeAnalytics.Engine.Archetypes.Members;
using CodeAnalytics.Engine.Archetypes.Types;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Analyze;

public sealed class AnalyzeStore : IDisposable
{
   public ClassArchetypeChunk ClassChunk { get; }
   public EnumArchetypeChunk EnumChunk { get; }
   public EnumValueArchetypeChunk EnumValueChunk { get; }
   public StructArchetypeChunk StructChunk { get; }
   public InterfaceArchetypeChunk InterfaceChunk { get; }
   
   public ConstructorArchetypeChunk ConstructorChunk { get; }
   public MethodArchetypeChunk MethodChunk { get; }
   public FieldArchetypeChunk FieldChunk { get; }
   public PropertyArchetypeChunk PropertyChunk { get; }

   public CollectorStore Inner { get; }

   public HashSet<NodeId> ContainingDeclarationIds { get; } = [];

   public AnalyzeStore(CollectorStore inner)
   {
      Inner = inner;

      ClassChunk = new ClassArchetypeChunk(inner.ComponentStore);
      EnumChunk = new EnumArchetypeChunk(inner.ComponentStore);
      EnumValueChunk = new EnumValueArchetypeChunk(inner.ComponentStore);
      StructChunk = new StructArchetypeChunk(inner.ComponentStore);
      InterfaceChunk = new InterfaceArchetypeChunk(inner.ComponentStore);
      
      ConstructorChunk = new ConstructorArchetypeChunk(inner.ComponentStore);
      MethodChunk = new MethodArchetypeChunk(inner.ComponentStore);
      FieldChunk = new FieldArchetypeChunk(inner.ComponentStore);
      PropertyChunk = new PropertyArchetypeChunk(inner.ComponentStore);
      
      InitContainingDeclarations();
   }

   private void InitContainingDeclarations()
   {
      InitContainingDeclaration(ref ClassChunk.Entries);
      InitContainingDeclaration(ref EnumChunk.Entries);
      InitContainingDeclaration(ref EnumValueChunk.Entries);
      InitContainingDeclaration(ref StructChunk.Entries);
      InitContainingDeclaration(ref InterfaceChunk.Entries);
      
      InitContainingDeclaration(ref ConstructorChunk.Entries);
      InitContainingDeclaration(ref MethodChunk.Entries);
      InitContainingDeclaration(ref FieldChunk.Entries);
      InitContainingDeclaration(ref PropertyChunk.Entries);
   }

   private void InitContainingDeclaration<TArchetype>(
      ref PooledList<TArchetype> archetypes)
      where TArchetype : IArchetype, IEquatable<TArchetype>
   {
      foreach (ref var archetype in archetypes)
      {
         ContainingDeclarationIds.Add(archetype.NodeId);
      }
   }
   
   public void Dispose()
   {
      ClassChunk.Dispose();
      EnumChunk.Dispose();
      StructChunk.Dispose();
      InterfaceChunk.Dispose();
      
      ConstructorChunk.Dispose();
      MethodChunk.Dispose();
      FieldChunk.Dispose();
      PropertyChunk.Dispose();
   }
}