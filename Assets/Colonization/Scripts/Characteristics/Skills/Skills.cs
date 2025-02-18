//Assets\Colonization\Scripts\Characteristics\Skills\Skills.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;
using Vurbiri.Localization;
using Vurbiri.UI;

namespace Vurbiri.Colonization.Characteristics
{
    [System.Serializable]
    public partial class Skills : IDisposable
    {
        public const int COUNT_SKILLS_MAX = 4;

        [SerializeField] private float _speedWalk = 0.45f;
        [SerializeField] private float _speedRun = 0.65f;
        [SerializeField] private int _blockCost = 2;
        [SerializeField] private int _blockValue = 10;
        [SerializeField] private SkillSettings[] _skillsSettings;
        
        [NonSerialized] private SkillUI[] _skillsUI;
        [NonSerialized] private EffectsHit[][] _effectsHits;
        [NonSerialized] private BlockUI _blockUI;

        public IReadOnlyList<SkillUI> SkillsUI => _skillsUI;
        public BlockUI BlockUI => _blockUI ??= new(_blockCost, _blockValue);
#if UNITY_EDITOR
        public IReadOnlyList<SkillSettings> List => _skillsSettings;
#endif

        public void CreateStates(Actor parent)
        {
            parent.AddMoveState(_speedWalk);
            parent.AddBlockState(_blockCost, _blockValue * ActorAbilityId.RATE_ABILITY);

            if (_effectsHits == null | _skillsUI == null)
                CreateEffectsAndUI(parent);

            int countSkills = Math.Min(_skillsSettings.Length, COUNT_SKILLS_MAX);

            for (int i = 0; i < countSkills; i++)
                parent.AddSkillState(_effectsHits[i], _skillsSettings[i], _speedRun, i);
        }

        private void CreateEffectsAndUI(Actor parent)
        {
            var hintTextColor = SceneData.Get<SettingsTextColor>();
            var language = SceneServices.Get<Language>();

            int countSkills = Math.Min(_skillsSettings.Length, COUNT_SKILLS_MAX);

            _effectsHits = new EffectsHit[countSkills][];
            _skillsUI = new SkillUI[countSkills];

            SkillSettings skillSettings; EffectsHitSettings effectsHitSettings;
            EffectsHit[] effectsHits;
            List<AEffectsUI>[] effectsSkillUI;
            for (int i = 0; i < countSkills; i++)
            {
                skillSettings = _skillsSettings[i];

                int countHits = skillSettings.effectsHits.Length;
                effectsHits = new EffectsHit[countHits];
                effectsSkillUI = new List<AEffectsUI>[] { new(countHits), new(countHits) };

                for (int j = 0, u = 0; j < countHits; j++)
                {
                    effectsHitSettings = skillSettings.effectsHits[j];
                    effectsHits[j] = effectsHitSettings.CreateEffectsHit(parent, i, u);
                    effectsHitSettings.CreateEffectsHitUI(hintTextColor, effectsSkillUI[0], effectsSkillUI[1]);
                    u += effectsHitSettings.Count;
                }
                skillSettings.ui.Init(language, hintTextColor, effectsSkillUI[0].ToArray(), effectsSkillUI[1].ToArray());

                _skillsUI[i] = skillSettings.ui;
                _effectsHits[i] = effectsHits;

#if !UNITY_EDITOR
                skill.ui = null;
                skill.effectsHits = null;
#endif
            }
        }

        public void Dispose()
        {
            _blockUI?.Dispose();

            if (_skillsUI == null)
                return;

            foreach (var skillUI in _skillsUI)
                skillUI.Dispose();
        }
    }
}
