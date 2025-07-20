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

   public CollectorStore Inner { get; }

   public AnalyzeStore(CollectorStore inner)
   {
      Inner = inner;

      ClassChunk = new ClassArchetypeChunk(inner.ComponentStore);
      EnumChunk = new EnumArchetypeChunk(inner.ComponentStore);
      StructChunk = new StructArchetypeChunk(inner.ComponentStore);
      InterfaceChunk = new InterfaceArchetypeChunk(inner.ComponentStore);
      
      ConstructorChunk = new ConstructorArchetypeChunk(inner.ComponentStore);
      MethodChunk = new MethodArchetypeChunk(inner.ComponentStore);
      FieldChunk = new FieldArchetypeChunk(inner.ComponentStore);
      PropertyChunk = new PropertyArchetypeChunk(inner.ComponentStore);
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