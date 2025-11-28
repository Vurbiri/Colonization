using System;
using UnityEngine;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;
using Object = UnityEngine.Object;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public abstract partial class ActorSettings : IDisposable
    {
        [SerializeField] private int _id;
        [SerializeField] private IdArray<ActorAbilityId, int> _abilities;
        [SerializeField] private Skills _skills;
        [SerializeField] private ActorSkin _prefabActorSkin;
        [SerializeField] private int _force;

        public abstract int TypeId { get; }
        public int Id { [Impl(256)] get => _id; }
        public Skills Skills { [Impl(256)] get => _skills; }
        public int Force { [Impl(256)] get => _force; }

        [Impl(256)] public AbilitiesSet<ActorAbilityId> GetAbilities() => new (_abilities, ActorAbilityId.SHIFT_ABILITY, ActorAbilityId.MAX_ID_SHIFT_ABILITY);

        [Impl(256)] public T InstantiateSkin<T>(Id<PlayerId> owner, Transform parent) where T : ActorSkin
        {
            T skin = (T)Object.Instantiate((Object)_prefabActorSkin, parent, false);
            skin.Init(owner, _skills);
            return skin;
        }

        [Impl(256)] public void Init() => _skills.Init(TypeId, _id);

        [Impl(256)] public void Dispose() => _skills.Dispose();
    }
}
