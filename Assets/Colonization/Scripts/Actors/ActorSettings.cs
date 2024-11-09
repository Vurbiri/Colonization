using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Actors
{
    [System.Serializable]
    public class ActorSettings
    {
        [SerializeField] private int _id;
        [SerializeField] private IdArray<ActorAbilityId, int> _abilities;
        [SerializeField] private Skills _skills;
        [SerializeField] private ActorSkin _prefabActorSkin;

        public int Id => _id;
        public AbilitiesSet<ActorAbilityId> Abilities => new(_abilities);
        public Skills Skills => _skills;

        public ActorSkin InstantiateActorSkin(Transform parent) => Object.Instantiate(_prefabActorSkin, parent);
    }
}
