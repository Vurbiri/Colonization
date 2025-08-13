using System;
using UnityEngine;
using Vurbiri.Collections;
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
        
        [NonSerialized] private ReadOnlyArray<SkillUI> _skillsUI;
        [NonSerialized] private ReadOnlyArray<HitEffects>[] _effectsHits;
        [NonSerialized] private BlockUI _blockUI;

        public BlockUI BlockUI => _blockUI;
        public ReadOnlyArray<SkillUI> SkillsUI => _skillsUI;
        public SkillSettings[] Settings => _skillsSettings;

        public void Init(int actorType, int actorId)
        {
            SkillSettings skill;
            int countSkills = Math.Min(_skillsSettings.Length, COUNT_SKILLS_MAX);

            _effectsHits = new ReadOnlyArray<HitEffects>[countSkills];

            if (actorType == ActorTypeId.Warrior)
            {
                _blockUI = new(_blockCost, _blockValue);

                var skillsUI = new SkillUI[countSkills];
                var colors = GameContainer.UI.Colors;

                for (int i = 0; i < countSkills; i++)
                {
                    skill = _skillsSettings[i];
                    skillsUI[i]     = skill.GetSkillUI(colors);
                    _effectsHits[i] = skill.CreateEffectsHit(actorType, actorId, i);
                }

                _skillsUI = new(skillsUI);
            }
            else
            {
                for (int i = 0; i < countSkills; i++)
                {
                    skill = _skillsSettings[i];
                    skill.RemoveSkillUI();
                    _effectsHits[i] = skill.CreateEffectsHit(actorType, actorId, i);
                }
            }
        }

        public void CreateStates(Actor actor)
        {
            actor.AddMoveState(_speedWalk);
            actor.AddBlockState(_blockCost, _blockValue << ActorAbilityId.SHIFT_ABILITY);

            int countSkills = Math.Min(_skillsSettings.Length, COUNT_SKILLS_MAX);

            actor.SetCountState(countSkills);
            for (int i = 0; i < countSkills; i++)
                actor.AddSkillState(_effectsHits[i], _skillsSettings[i], _speedRun, i);
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

        public void SetTypeActor_Ed(int type)
        {
            for (int i = 0; i < _skillsSettings.Length; i++)
                _skillsSettings[i].typeActor_ed = type;
        }
#endif
    }
}
