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

        public int Id => _id;
        public AbilitiesSet<ActorAbilityId> Abilities => new(_abilities);
        public Skills Skills => _skills;


    }
}
