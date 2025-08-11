using System;
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

                var colors = GameContainer.UI.Colors;
                int countSkills = Math.Min(_skillsSettings.Length, COUNT_SKILLS_MAX);
                SkillUI[] skillsUI = new SkillUI[countSkills];

                for (int i = 0; i < countSkills; i++)
                    skillsUI[i] = _skillsSettings[i].GetSkillUI(colors);

                return _skillsUI = new(skillsUI);
            }
        }
        public SkillSettings[] Settings => _skillsSettings;

        public void CreateStates(Actor actor)
        {
            actor.AddMoveState(_speedWalk);
            actor.AddBlockState(_blockCost, _blockValue << ActorAbilityId.SHIFT_ABILITY);

            int countSkills = Math.Min(_skillsSettings.Length, COUNT_SKILLS_MAX);
            actor.SetCountState(countSkills);

            if (_effectsHits != null)
            {
                for (int i = 0; i < countSkills; i++)
                    actor.AddSkillState(_effectsHits[i], _skillsSettings[i], _speedRun, i);
            }
            else
            {
                _effectsHits = new HitEffects[countSkills][];
                for (int i = 0; i < countSkills; i++)
                    _effectsHits[i] = actor.AddSkillState(_skillsSettings[i], _speedRun, i);
            }
        }

        public void Dispose()
        {
            _blockUI?.Dispose();

            if (_skillsUI != null)
            {
                foreach (var skillUI in _skillsUI)
                    skillUI.Dispose();
            }
        }

#if UNITY_EDITOR
        [SerializeField] private int _swapA = -1;
        [SerializeField] private int _swapB = -1;

        public void Swap_Ed()
        {
            int count = _skillsSettings.Length;
            if (_swapA != _swapB && _swapA >= 0 & _swapB >= 0 && _swapA < count & _swapB < count)
                (_skillsSettings[_swapA], _skillsSettings[_swapB]) = (_skillsSettings[_swapB], _skillsSettings[_swapA]);

            _swapA = _swapB = -1;
        }
#endif
    }
}
