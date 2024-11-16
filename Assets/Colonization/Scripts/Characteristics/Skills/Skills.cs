using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;
using static Vurbiri.Colonization.Actors.Actor;

namespace Vurbiri.Colonization.Characteristics
{
    [System.Serializable]
    public partial class Skills
    {
        [SerializeField] private float _speedWalk = 0.45f;
        [SerializeField] private float _speedRun = 0.65f;
        [SerializeField] private SkillSettings[] _skillsSettings;
        
        [NonSerialized] private List<SkillUI> _skillsUI;
        [NonSerialized] private AEffect[][] _effects;

        public MoveState GetMoveSate(Actor parent) => new(_speedWalk, parent);

        public List<ASkillState> GetSkillSates(Actor parent)
        {
            int count = _skillsSettings.Length;
            List<ASkillState> skillStates = new(count);
            _effects ??= new AEffect[count][];

            SkillSettings skill; AEffect[] effectsSkill;
            for (int i = 0, id = 0; i < count; i++)
            {
                skill = _skillsSettings[i];
                if (skill == null || !skill.isValid)
                    throw new ArgumentNullException("SkillSettings и/или AnimationClipSettings равны null!");


                effectsSkill = _effects[id];
                if(effectsSkill == null)
                {
                    int countEffects = skill.effects.Length;
                    effectsSkill = new AEffect[countEffects];
                    for (int j = 0; j < countEffects; j++)
                        effectsSkill[j] = skill.effects[j].CreateEffect();
                    _effects[id] = effectsSkill;
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
