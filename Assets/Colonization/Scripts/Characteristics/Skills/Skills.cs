namespace Vurbiri.Colonization.Characteristics
{
    using Actors;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Vurbiri.Colonization.UI;
    using static Actors.Actor;

    [System.Serializable]
    public partial class Skills
    {
        [SerializeField] private float _speedWalk = 0.45f;
        [SerializeField] private float _speedRun = 0.65f;
        [SerializeField] private SkillSettings[] _skillsSettings;
        
        [NonSerialized] private List<SkillUI> _skillsUI;
        [NonSerialized] private List<List<AEffect>> _effects;

        public MoveState GetMoveSate(Actor parent) => new(_speedWalk, parent);

        public List<ASkillState> GetSkillSates(Actor parent)
        {
            int count = _skillsSettings.Length;
            List<ASkillState> skillStates = new(count);
            _effects ??= new(count);

            SkillSettings skill; List<AEffect> effectsSkill;
            for (int i = 0, id = 0; i < count; i++)
            {
                skill = _skillsSettings[i];
                if (skill == null || !skill.isValid)
                    throw new ArgumentNullException("SkillSettings и/или AnimationClipSettings равны null!");

                effectsSkill = _effects[i];
                if(effectsSkill == null)
                {
                    int countEffects = skill.effects.Length;
                    effectsSkill = new(countEffects);
                    for (int j = 0; j < countEffects; j++)
                        effectsSkill.Add(skill.effects[j].CreateEffect());
                    _effects.Add(effectsSkill);
                }

                if (skill.target == TargetOfEffectId.Self)
                {
                    skillStates.Add(new SelfBuffState(parent, effectsSkill, skill.settings, id++));
                    continue;
                }
                if (skill.isMove)
                {
                    skillStates.Add(new AttackState(parent, skill.target, effectsSkill, skill.range, _speedRun, skill.settings, id++));
                    continue;
                }
                
                skillStates.Add(new SpellState(parent, skill.target, effectsSkill, skill.settings, id++));
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

                skill.ui.SetEffects(skill.effects);
                _skillsUI.Add(skill.ui);
            }

            return _skillsUI;
        }
    }
}
