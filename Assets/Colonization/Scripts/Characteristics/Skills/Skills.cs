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
        [SerializeField] private float _speedWalk; // = 0.45f;
        [SerializeField] private float _speedRun; // = 0.65f;
        [SerializeField] private int _blockCost; // = 2;
        [SerializeField] private int _blockValue; // = 10;
        [SerializeField] private SkillSettings[] _skillsSettings;

        [SerializeField] private ReadOnlyArray<string> _hitSfxNames;
        [SerializeField] private ReadOnlyArray<AnimationTime> _timings;

        [NonSerialized] private ASkillUI _specSkillUI;
        [NonSerialized] private ReadOnlyArray<SkillUI> _skillsUI;

        public ASkillUI SpecSkillUI => _specSkillUI;
        public ReadOnlyArray<SkillUI> SkillsUI => _skillsUI;
        public ReadOnlyArray<string> HitSfxNames => _hitSfxNames;
        public ReadOnlyArray<AnimationTime> Timings => _timings;

        public void Init(int actorType, int actorId)
        {
            int countSkills = _skillsSettings.Length;

            var skillsUI = new SkillUI[countSkills];
            var colors = GameContainer.UI.Colors;
            var separator = new SeparatorEffectUI(colors);

            for (int i = 0; i < countSkills; i++)
                skillsUI[i] = _skillsSettings[i].Init(colors, separator, actorType, actorId, i);

            _skillsUI = new(skillsUI);

            if (actorType == ActorTypeId.Warrior)
                _specSkillUI = new BlockUI(colors, separator, _blockCost, _blockValue);
            else
                _specSkillUI = new SpecSkillUI(colors, separator);
        }

        public void CreateStates(Actor actor)
        {
            actor.AddMoveState(_speedWalk);
            actor.AddSpecSkillState(_blockCost, _blockValue << ActorAbilityId.SHIFT_ABILITY);

            int countSkills = _skillsSettings.Length;

            actor.SetCountState(countSkills);
            for (int i = 0; i < countSkills; i++)
                actor.AddSkillState(_skillsSettings[i], _speedRun, i);
        }

        public void Dispose()
        {
            _specSkillUI?.Dispose();

            if (_skillsUI != null)
            {
                for (int i = 0; i < _skillsUI.Count; i++)
                    _skillsUI[i].Dispose();
            }
        }

#if UNITY_EDITOR

        public const int COUNT_SKILLS_MAX = 4;
        private static readonly string[] A_SKILLS = { "A_Skill_0", "A_Skill_1", "A_Skill_2", "A_Skill_3" };

        [SerializeField] private int _swapA = -1;
        [SerializeField] private int _swapB = -1;
        [SerializeField] private string _specSkillName_ed = "Block";

        public void OnValidate(int type)
        {
            _specSkillName_ed = type == ActorTypeId.Warrior ? "Block" : "Spec Skill";

            if (_skillsSettings.Length > COUNT_SKILLS_MAX)
                Array.Resize(ref _skillsSettings, COUNT_SKILLS_MAX);

            for (int i = 0; i < _skillsSettings.Length; i++)
                _skillsSettings[i].typeActor_ed = type;
        }

        public bool UpdateName_Ed(string oldName, string newName)
        {
            bool changed = false;
            for (int i = 0; i < _skillsSettings.Length; i++)
            {
                if (_skillsSettings[i].hitSFXName_ed.Update_Ed(oldName, newName))
                {
                    changed = true;
                    _hitSfxNames.SetValue_EditorOnly(i, newName);
                }
            }
            return changed;
        }

        public void UpdateAnimation_Ed(AnimatorOverrideController animator)
        {
            if (animator == null) return;
            
            int countSkills = _skillsSettings.Length;
            if (_swapA != _swapB && _swapA >= 0 & _swapB >= 0 && _swapA < countSkills & _swapB < countSkills)
                (_skillsSettings[_swapA], _skillsSettings[_swapB]) = (_skillsSettings[_swapB], _skillsSettings[_swapA]);
            _swapA = _swapB = -1;

            var sfxNames = new string[countSkills]; var timings = new AnimationTime[countSkills];
            SkillSettings skillSettings; int index;

            for (index = 0; index < countSkills; index++)
            {
                skillSettings = _skillsSettings[index];

                sfxNames[index] = skillSettings.hitSFXName_ed;
                timings[index] = new(skillSettings.clipSettings_ed);

                if (animator[A_SKILLS[index]] != skillSettings.clipSettings_ed.clip)
                    animator[A_SKILLS[index]] = skillSettings.clipSettings_ed.clip;
            }

            for (; index < COUNT_SKILLS_MAX; index++)
                if (animator[A_SKILLS[index]].name != A_SKILLS[index])
                    animator[A_SKILLS[index]] = null;

            _hitSfxNames = sfxNames;
            _timings = timings;
        }
#endif
    }
}
