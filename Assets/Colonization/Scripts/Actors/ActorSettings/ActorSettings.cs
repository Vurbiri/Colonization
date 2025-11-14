using System;
using UnityEngine;
using Vurbiri.Collections;

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
        public int Id => _id;
        public AbilitiesSet<ActorAbilityId> Abilities => new(_abilities, ActorAbilityId.SHIFT_ABILITY, ActorAbilityId.MAX_ID_SHIFT_ABILITY);
        public Skills Skills => _skills;
        public int Force => _force;

        public T InstantiateSkin<T>(Id<PlayerId> owner, Transform parent) where T : ActorSkin
        {
            T skin = (T)Object.Instantiate((Object)_prefabActorSkin, parent, false);
            skin.Init(owner, _skills);
            return skin;
        }

        public void Init() => _skills.Init(TypeId, _id);

        public void Dispose() => _skills.Dispose();
    }
}
