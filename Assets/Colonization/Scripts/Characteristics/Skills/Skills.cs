//Assets\Colonization\Scripts\Characteristics\Skills\Skills.cs
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
        [SerializeField] private int _blockCost = 2;
        [SerializeField] private int _blockValue = 100;
        [SerializeField] private SkillSettings[] _skillsSettings;
        
        [NonSerialized] private SkillUI[] _skillsUI;
        [NonSerialized] private AEffect[][] _effects;
        [NonSerialized] private BlockUI _blockUI;

        public IReadOnlyList<SkillUI> SkillsUI => _skillsUI;
        public BlockUI BlockUI => _blockUI ??= new(_blockCost, _blockValue);

        public MoveState GetMoveSate(Actor parent) => new(_speedWalk, parent);
        public BlockState GetBlockState(Actor parent) => new(_blockCost, _blockValue, parent);

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
            _blockUI?.Dispose();

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
            for (int i = 0, j; i < count; i++)
            {
                skill = _skillsSettings[i];
                skill.SetTiming();

                int countEffects = skill.effects.Length;
                effectsSkill = new AEffect[countEffects];
                effectsSkillUI = new AEffectsUI[countEffects];

                for (j = 0; j < countEffects; j++)
                {
                    effect = skill.effects[j];
                    effectsSkill[j] = effect.CreateEffect(new(parent.TypeId, parent.Id, i, j));
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
            if (skill.target == TargetOfSkill.Self)
                return new SelfBuffState(parent, _effects[id], skill.settings, id);

            if (skill.isMove)
                return new AttackState(parent, skill.target, _effects[id], skill.range, _speedRun, skill.settings, id);

            return new SpellState(parent, skill.target, _effects[id], skill.isTargetReact, skill.settings, id);
        }
    }
}
