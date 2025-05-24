using System;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    [System.Serializable]
    public abstract class ActorSettings : IDisposable
    {
        [SerializeField] private int _id;
        [SerializeField] private IdArray<ActorAbilityId, int> _abilities;
        [SerializeField] private Skills _skills;
        [SerializeField] private ActorSkin _prefabActorSkin;

        public abstract int TypeId { get; }
        public int Id => _id;
        public AbilitiesSet<ActorAbilityId> Abilities => new(_abilities, ActorAbilityId.SHIFT_ABILITY, ActorAbilityId.MAX_ID_SHIFT_ABILITY);
        public Skills Skills => _skills;

        public ActorSkin InstantiateActorSkin(Transform parent) => UnityEngine.Object.Instantiate(_prefabActorSkin, parent).Init();

        public void Dispose()
        {
            _skills.Dispose();
        }

#if UNITY_EDITOR
        public ActorSkin PrefabSkin => _prefabActorSkin;
#endif
    }
}
