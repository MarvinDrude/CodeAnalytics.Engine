using CodeAnalytics.Engine.Archetypes.Members;
using CodeAnalytics.Engine.Archetypes.Types;
using CodeAnalytics.Engine.Collectors;

namespace CodeAnalytics.Engine.Analyze;

public sealed class AnalyzeStore : IDisposable
{
   public ClassArchetypeChunk ClassChunk { get; }
   public EnumArchetypeChunk EnumChunk { get; }
   public StructArchetypeChunk StructChunk { get; }
   public InterfaceArchetypeChunk InterfaceChunk { get; }
   
   public ConstructorArchetypeChunk ConstructorChunk { get; }
   public MethodArchetypeChunk MethodChunk { get; }
   public FieldArchetypeChunk FieldChunk { get; }
   public PropertyArchetypeChunk PropertyChunk { get; }

   private readonly CollectorStore _store;
   
   public AnalyzeStore(CollectorStore store)
   {
      _store = store;

      ClassChunk = new ClassArchetypeChunk(store.ComponentStore);
      EnumChunk = new EnumArchetypeChunk(store.ComponentStore);
      StructChunk = new StructArchetypeChunk(store.ComponentStore);
      InterfaceChunk = new InterfaceArchetypeChunk(store.ComponentStore);
      
      ConstructorChunk = new ConstructorArchetypeChunk(store.ComponentStore);
      MethodChunk = new MethodArchetypeChunk(store.ComponentStore);
      FieldChunk = new FieldArchetypeChunk(store.ComponentStore);
      PropertyChunk = new PropertyArchetypeChunk(store.ComponentStore);
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