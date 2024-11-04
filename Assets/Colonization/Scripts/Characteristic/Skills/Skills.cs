using UnityEngine;

namespace Vurbiri.Colonization
{
    using Actors;
    using System;
    using System.Collections.Generic;
    using Vurbiri.Colonization.UI;
    using static Actors.Actor;

    [System.Serializable]
    public partial class Skills
    {
        [SerializeField] private float _speedWalk = 0.45f;
        [SerializeField] private float _speedRun = 0.65f;
        [SerializeField] private AttackSkill[] _attackSkills;
        
        [NonSerialized] private List<AttackSkillUI> _attackSkillsUI;

        public MoveState GetMoveSate(Actor parent) => new(_speedWalk, parent);

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

                attackStates.Add(new(parent, skill.percentDamage, _speedRun, skill.settings, id++));
            }

            return attackStates;
        }

        public List<AttackSkillUI> GetAttackSkillsUI()
        {
            if(_attackSkillsUI != null)
                return _attackSkillsUI;

            int count = _attackSkills.Length;
            _attackSkillsUI = new(count);
            
            AttackSkill skill;
            for (int i = 0; i < count; i++)
            {
                skill = _attackSkills[i];
                if (skill == null || !skill.isValid)
                    continue;

                _attackSkillsUI.Add(skill.ui);
            }

            return _attackSkillsUI;
        }
    }
}
