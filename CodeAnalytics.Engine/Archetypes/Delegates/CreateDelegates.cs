using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Inerfaces;

namespace CodeAnalytics.Engine.Archetypes.Delegates;

public delegate void CreateArchetype<TArchetype, TC1, TC2>(ref TC1 tcOne, ref TC2 tcTwo, out TArchetype archetype)
   where TArchetype : IArchetype, IEquatable<TArchetype>
   where TC1 : IComponent, IEquatable<TC1>
   where TC2 : IComponent, IEquatable<TC2>;
   
public delegate void CreateArchetype<TArchetype, TC1, TC2, TC3>(ref TC1 tcOne, ref TC2 tcTwo, ref TC3 tcThree, out TArchetype archetype)
   where TArchetype : IArchetype, IEquatable<TArchetype>
   where TC1 : IComponent, IEquatable<TC1>
   where TC2 : IComponent, IEquatable<TC2>
   where TC3 : IComponent, IEquatable<TC3>;