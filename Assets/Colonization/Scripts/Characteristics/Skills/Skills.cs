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

        public BlockUI BlockUI => _blockUI ??= new(_blockCost, _blockValue);
        public IReadOnlyList<SkillUI> SkillsUI
        {
            get
            {
                if (_skillsUI != null)  
                    return _skillsUI;

                var hintTextColor = SceneData.Get<SettingsTextColor>();
                var language = SceneServices.Get<Language>();
                int countSkills = Math.Min(_skillsSettings.Length, COUNT_SKILLS_MAX);
                _skillsUI = new SkillUI[countSkills];

                for (int i = 0; i < countSkills; i++)
                    _skillsUI[i] = _skillsSettings[i].GetSkillUI(language, hintTextColor);

                return _skillsUI;
            }
        }
        public IReadOnlyList<SkillSettings> Settings => _skillsSettings;

        public void CreateStates(Actor parent)
        {
            parent.AddMoveState(_speedWalk);
            parent.AddBlockState(_blockCost, _blockValue * ActorAbilityId.RATE_ABILITY);

            int countSkills = Math.Min(_skillsSettings.Length, COUNT_SKILLS_MAX);

            if (_effectsHits != null)
            {
                for (int i = 0; i < countSkills; i++)
                    parent.AddSkillState(_effectsHits[i], _skillsSettings[i], _speedRun, i);

                return;
            }

            _effectsHits = new EffectsHit[countSkills][];
            for (int i = 0; i < countSkills; i++)
                _effectsHits[i] = parent.AddSkillState(_skillsSettings[i], _speedRun, i);
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
