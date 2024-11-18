using System;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    [System.Serializable]
    public class ActorSettings : IDisposable
    {
        [SerializeField] private int _id;
        [SerializeField] private IdArray<ActorAbilityId, int> _abilities;
        [SerializeField] private Skills _skills;
        [SerializeField] private ActorSkin _prefabActorSkin;

        public int Id => _id;
        public AbilitiesSet<ActorAbilityId> Abilities => new(_abilities);
        public Skills Skills => _skills;

        public ActorSkin InstantiateActorSkin(Transform parent) => UnityEngine.Object.Instantiate(_prefabActorSkin, parent);


        public void Dispose()
        {
            _skills.Dispose();
        }
    }
}
