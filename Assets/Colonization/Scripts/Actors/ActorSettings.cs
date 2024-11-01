using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Actors
{
    [System.Serializable]
    public class ActorSettings
    {
        [SerializeField] private int _id;
        [SerializeField] private IdArray<ActorStateId, int> _states;
        [SerializeField] private Skills _skills;

        public int Id => _id;
        public Skills Skills => _skills;
    }
}
