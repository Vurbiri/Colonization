using UnityEngine;

namespace Vurbiri.Colonization
{
    using Actors;
    using static Actors.Actor;

    [System.Serializable]
    public partial class Skills
    {
        [SerializeField] MoveSkill _moveSkill;


        public MoveState GetMoveSate(Actor parent) => new(_moveSkill.speed, parent);
    }
}
