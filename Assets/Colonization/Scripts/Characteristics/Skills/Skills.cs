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
        public const int COUNT_SKILLS_MAX = 4;

        [SerializeField] private float _speedWalk = 0.45f;
        [SerializeField] private float _speedRun = 0.65f;
        [SerializeField] private int _blockCost = 2;
        [SerializeField] private int _blockValue = 100;
        [SerializeField] private SkillSettings[] _skillsSettings;
        
        [NonSerialized] private SkillUI[] _skillsUI;
        [NonSerialized] private EffectsHit[][] _effectsHits;
        [NonSerialized] private BlockUI _blockUI;

        public IReadOnlyList<SkillUI> SkillsUI => _skillsUI;
        public BlockUI BlockUI => _blockUI ??= new(_blockCost, _blockValue);

        public MoveState GetMoveSate(Actor parent) => new(_speedWalk, parent);
        public BlockState GetBlockState(Actor parent) => new(_blockCost, _blockValue, parent);

        public List<ASkillState> GetSkillSates(Actor parent)
        {
            if(_effectsHits == null | _skillsUI == null)
                return GetAndCreateSkills(parent);

            int countSkills = Math.Min(_skillsSettings.Length, COUNT_SKILLS_MAX);
            List<ASkillState> skillStates = new(countSkills);

            for (int i = 0; i < countSkills; i++)
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
            var hintTextColor = SceneData.Get<SettingsTextColor>();
            var language = SceneServices.Get<Language>();

            int countSkills = Math.Min(_skillsSettings.Length, COUNT_SKILLS_MAX);
            List<ASkillState> skillStates = new(countSkills);

            _effectsHits = new EffectsHit[countSkills][];
            _skillsUI = new SkillUI[countSkills];

            SkillSettings skill; EffectsHitSettings packSettings;
            EffectsHit[] effectsHits; 
            List<AEffectsUI> effectsSkillUI;
            for (int i = 0; i < countSkills; i++)
            {
                skill = _skillsSettings[i];

                int countHits = skill.effectsHits.Length;
                effectsHits = new EffectsHit[countHits];
                effectsSkillUI = new(countHits << 1);

                for (int j = 0, u = 0; j < countHits; j++)
                {
                    packSettings = skill.effectsHits[j];
                    effectsHits[j] = packSettings.CreateHit(parent, i, u);
                    effectsSkillUI.AddRange(packSettings.CreateEffectsUI(hintTextColor));
                    u += packSettings.Count;
                }
                skill.ui.Init(language, hintTextColor, effectsSkillUI.ToArray());

                _skillsUI[i] = skill.ui;
                _effectsHits[i] = effectsHits;

                skillStates.Add(CreateState(parent, skill, i));

#if !UNITY_EDITOR
                skill.ui = null;
                skill.effectsHits = null;
#endif
            }

            return skillStates;
        }

        private ASkillState CreateState(Actor parent, SkillSettings skill, int id)
        {
            if (skill.target == TargetOfSkill.Self)
                return new SelfSkillState(parent, _effectsHits[id], skill.cost, id);

            if (skill.isMove)
                return new SkillState(parent, skill.target, _effectsHits[id], skill.isTargetReact, skill.range, _speedRun, skill.cost, id);

            return new RangeSkillState(parent, skill.target, _effectsHits[id], skill.isTargetReact, skill.cost, id);
        }

#if UNITY_EDITOR
        public IReadOnlyList<SkillSettings> List => _skillsSettings;
#endif
    }
}
