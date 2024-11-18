using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;
using Vurbiri.Localization;
using Vurbiri.UI;
using static Vurbiri.Colonization.Actors.Actor;

namespace Vurbiri.Colonization.Characteristics
{
    [System.Serializable]
    public partial class Skills : IDisposable
    {
        [SerializeField] private float _speedWalk = 0.45f;
        [SerializeField] private float _speedRun = 0.65f;
        [SerializeField] private SkillSettings[] _skillsSettings;
        
        [NonSerialized] private SkillUI[] _skillsUI;
        [NonSerialized] private AEffect[][] _effects;

        public IReadOnlyList<SkillUI> SkillsUI => _skillsUI;

        public MoveState GetMoveSate(Actor parent) => new(_speedWalk, parent);

        public List<ASkillState> GetSkillSates(Actor parent)
        {
            if(_effects == null | _skillsUI == null)
                return GetAndCreateSkills(parent);

            int count = _skillsSettings.Length;
            List<ASkillState> skillStates = new(count);

            for (int i = 0; i < count; i++)
                skillStates.Add(CreateState(parent, _skillsSettings[i], i));

            return skillStates;
        }

        public void Dispose()
        {
            if (_skillsUI == null)
                return;

            foreach (var skillUI in _skillsUI)
                skillUI.Dispose();
        }

        private List<ASkillState> GetAndCreateSkills(Actor parent)
        {
            var hintTextColor = SceneData.Get<HintTextColor>();
            var language = SceneServices.Get<Language>();

            int count = _skillsSettings.Length;
            List<ASkillState> skillStates = new(count);

            _effects = new AEffect[count][];
            _skillsUI = new SkillUI[count];

            SkillSettings skill; EffectSettings effect;
            AEffect[] effectsSkill; AEffectsUI[] effectsSkillUI;
            for (int i = 0; i < count; i++)
            {
                skill = _skillsSettings[i];

                int countEffects = skill.effects.Length;
                effectsSkill = new AEffect[countEffects];
                effectsSkillUI = new AEffectsUI[countEffects];

                for (int j = 0; j < countEffects; j++)
                {
                    effect = skill.effects[j];
                    effectsSkill[j] = effect.CreateEffect();
                    effectsSkillUI[j] = effect.CreateEffectUI(hintTextColor);
                }
                skill.ui.Init(language, hintTextColor, effectsSkillUI);

                _skillsUI[i] = skill.ui;
                _effects[i] = effectsSkill;

                skillStates.Add(CreateState(parent, skill, i));

#if !UNITY_EDITOR
                skill.ui = null;
                skill.effects = null;
#endif

            }

            return skillStates;
        }

        private ASkillState CreateState(Actor parent, SkillSettings skill, int id)
        {
            if (skill.target == TargetOfSkillId.Self)
                return new SelfBuffState(parent, _effects[id], skill.settings, id);

            if (skill.isMove)
                return new AttackState(parent, skill.target, _effects[id], skill.range, _speedRun, skill.settings, id);

            return new SpellState(parent, skill.target, _effects[id], skill.settings, id);
        }
    }
}
