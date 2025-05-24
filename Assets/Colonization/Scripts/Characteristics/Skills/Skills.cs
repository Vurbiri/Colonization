using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization.Characteristics
{
    [System.Serializable]
    public class Skills : IDisposable
    {
        public const int COUNT_SKILLS_MAX = 4;

        [SerializeField] private float _speedWalk = 0.45f;
        [SerializeField] private float _speedRun = 0.65f;
        [SerializeField] private int _blockCost = 2;
        [SerializeField] private int _blockValue = 10;
        [SerializeField] private SkillSettings[] _skillsSettings;
        
        [NonSerialized] private ReadOnlyCollection<SkillUI> _skillsUI;
        [NonSerialized] private HitEffects[][] _effectsHits;
        [NonSerialized] private BlockUI _blockUI;

        public BlockUI BlockUI => _blockUI ??= new(_blockCost, _blockValue);
        public ReadOnlyCollection<SkillUI> SkillsUI
        {
            get
            {
                if (_skillsUI != null)  
                    return _skillsUI;

                var colors = SceneContainer.Get<ProjectColors>();
                int countSkills = Math.Min(_skillsSettings.Length, COUNT_SKILLS_MAX);
                SkillUI[] skillsUI = new SkillUI[countSkills];

                for (int i = 0; i < countSkills; i++)
                    skillsUI[i] = _skillsSettings[i].GetSkillUI(colors);

                return _skillsUI = new(skillsUI);
            }
        }
        public IReadOnlyList<SkillSettings> Settings => _skillsSettings;

        public void CreateStates(Actor parent)
        {
            parent.AddMoveState(_speedWalk);
            parent.AddBlockState(_blockCost, _blockValue << ActorAbilityId.SHIFT_ABILITY);

            int countSkills = Math.Min(_skillsSettings.Length, COUNT_SKILLS_MAX);

            if (_effectsHits != null)
            {
                for (int i = 0; i < countSkills; i++)
                    parent.AddSkillState(_effectsHits[i], _skillsSettings[i], _speedRun, i);

                return;
            }

            _effectsHits = new HitEffects[countSkills][];
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
