using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Analyze.Searchers;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;

namespace CodeAnalytics.Engine.Analyze.Searchers;

public abstract class BaseArchetypeSearcher
{
   public List<IArchetype> Results { get; } = [];
   
   protected readonly BaseSearchOptions _options;
   protected readonly AnalyzeStore _store;
   
   public BaseArchetypeSearcher(
      AnalyzeStore store,
      BaseSearchOptions options)
   {
      _options = options;
      _store = store;
   }

   public void Search()
   {
      if (_options.Classes) SearchArchetypes(ref _store.ClassChunk.Entries, Predicate);
      if (CheckMaxResults()) return;
      if (_options.Enums) SearchArchetypes(ref _store.EnumChunk.Entries, Predicate);
      if (CheckMaxResults()) return;
      if (_options.Interfaces) SearchArchetypes(ref _store.InterfaceChunk.Entries, Predicate);
      if (CheckMaxResults()) return;
      if (_options.Structs) SearchArchetypes(ref _store.StructChunk.Entries, Predicate);
      if (CheckMaxResults()) return;
      
      if (_options.Methods) SearchArchetypes(ref _store.MethodChunk.Entries, Predicate);
      if (CheckMaxResults()) return;
      if (_options.Constructors) SearchArchetypes(ref _store.ConstructorChunk.Entries, Predicate);
      if (CheckMaxResults()) return;
      if (_options.Fields) SearchArchetypes(ref _store.FieldChunk.Entries, Predicate);
      if (CheckMaxResults()) return;
      if (_options.Properties) SearchArchetypes(ref _store.PropertyChunk.Entries, Predicate);
      if (CheckMaxResults()) return;
   }

   private void SearchArchetypes<TArchetype>(ref PooledList<TArchetype> archetypes, PredicateDelegate<TArchetype> checkFunc)
      where TArchetype : IArchetype, IEquatable<TArchetype>
   {
      foreach (ref var archetype in archetypes)
      {
         if (checkFunc(ref archetype))
         {
            Results.Add(archetype);
         }
      }
   }

   private bool CheckMaxResults()
   {
      if (_options.MaxResults > Results.Count)
      {
         return false;
      }

      if (_options.TrimResults && Results.Count > _options.MaxResults)
      {
         Results.RemoveRange(_options.MaxResults, Results.Count - _options.MaxResults);
      }
      
      return true;
   }

   protected virtual bool Predicate(ref ClassArchetype archetype) => false;
   protected virtual bool Predicate(ref EnumArchetype archetype) => false;
   protected virtual bool Predicate(ref InterfaceArchetype archetype) => false;
   protected virtual bool Predicate(ref StructArchetype archetype) => false;
   
   protected virtual bool Predicate(ref MethodArchetype archetype) => false;
   protected virtual bool Predicate(ref ConstructorArchetype archetype) => false;
   protected virtual bool Predicate(ref FieldArchetype archetype) => false;
   protected virtual bool Predicate(ref PropertyArchetype archetype) => false;
   
   private delegate bool PredicateDelegate<TArchetype>(ref TArchetype archetype);
}