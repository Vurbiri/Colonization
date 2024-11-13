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
        [SerializeField] private SkillSettings[] _skillsSettings;
        
        [NonSerialized] private List<SkillUI> _skillsUI;

        public MoveState GetMoveSate(Actor parent) => new(_speedWalk, parent);

        public List<ASkillState> GetSkillSates(Actor parent)
        {
            int count = _skillsSettings.Length;
            List<ASkillState> skillStates = new(count);

            SkillSettings skill;
            for (int i = 0, id = 0; i < count; i++)
            {
                skill = _skillsSettings[i];
                if (skill == null || !skill.isValid)
                    continue;

                if (skill.isMove)
                    skillStates.Add(new AttackState(parent, skill.percentDamage, skill.range, _speedRun, skill.settings, id++));
                else
                    skillStates.Add(new SpellState(parent, skill.percentDamage, skill.settings, id++));
            }

            return skillStates;
        }

        public List<SkillUI> GetAttackSkillsUI()
        {
            if(_skillsUI != null)
                return _skillsUI;

            int count = _skillsSettings.Length;
            _skillsUI = new(count);
            
            SkillSettings skill;
            for (int i = 0; i < count; i++)
            {
                skill = _skillsSettings[i];
                if (skill == null || !skill.isValid)
                    continue;

                skill.ui.SetEffects(skill.settings.effects);
                _skillsUI.Add(skill.ui);
            }

            return _skillsUI;
        }
    }
}
