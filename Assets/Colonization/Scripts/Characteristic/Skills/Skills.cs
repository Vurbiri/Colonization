using UnityEngine;

namespace Vurbiri.Colonization
{
    using Actors;
    using System.Collections.Generic;
    using static Actors.Actor;

    [System.Serializable]
    public partial class Skills
    {
        [SerializeField] MoveSkill _moveSkill;
        [SerializeField] AttackSkill[] _attackSkills = new AttackSkill[1];

        public MoveState GetMoveSate(Actor parent) => new(_moveSkill.speed, parent);

        public List<AttackState> GetAttackSates(Actor parent)
        {
            int count = _attackSkills.Length;
            List<AttackState> attackStates = new(count);

            AttackSkill skill;
            for (int i = 0, id = 0; i < count; i++)
            {
                skill = _attackSkills[i];
                if (skill == null || !skill.isValid)
                    continue;

                attackStates.Add(new(parent, skill.settings, id++));
            }

            return attackStates;
        }
    }
}
