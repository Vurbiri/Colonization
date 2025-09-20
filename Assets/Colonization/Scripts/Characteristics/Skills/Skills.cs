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
        [SerializeField] private float _runSpeed; // = 0.6f;
        [SerializeField] private float _walkSpeed; // = 0.6f;
        [SerializeField] private float _speed; // = 0.6f;
        [SerializeField] private SpecSkillSettings _specSkillSettings;
        [SerializeField] private SkillSettings[] _skillsSettings;

        [SerializeField] private Array<string> _hitSfxNames;
        [SerializeField] private Array<AnimationTime> _timings;

        [NonSerialized] private Array<SkillUI> _skillsUI;

        public ReadOnlyArray<SkillUI> SkillsUI => _skillsUI;
        public ReadOnlyArray<string> HitSfxNames => _hitSfxNames;
        public ReadOnlyArray<AnimationTime> Timings => _timings;

        public SpecSkillSettings Spec => _specSkillSettings;

        public void Init(int actorType, int actorId)
        {
            int countSkills = _skillsSettings.Length;

            _skillsUI = new (countSkills);
            var colors = GameContainer.UI.Colors;
            var separator = new SeparatorEffectUI(colors);

            for (int i = 0; i < countSkills; i++)
                _skillsUI[i] = _skillsSettings[i].Init(colors, separator, actorType, actorId, i);

            _specSkillSettings.Init(colors, separator, actorType, actorId);
        }

        public void CreateStates<TActor, TSkin>(Actor.AStates<TActor, TSkin> states) where TActor : Actor where TSkin : ActorSkin
        {
            states.AddMoveState(_walkSpeed);
            states.AddSpecSkillState(_specSkillSettings, _runSpeed, _walkSpeed);

            int countSkills = _skillsSettings.Length;
            states.SetCountState(countSkills);
            for (int i = 0; i < countSkills; i++)
                states.AddSkillState(_skillsSettings[i], _runSpeed, i);
        }

        public void Dispose()
        {
            _specSkillSettings.Dispose();

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

        public void OnValidate(int type, int id)
        {
            _specSkillSettings.OnValidate(type, id);

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
                    _hitSfxNames[i] = newName;
                }
            }
            return changed;
        }

        public void UpdateAnimation_Ed(AnimatorOverrideController animator)
        {
            if (animator == null) return;

            _specSkillSettings.UpdateAnimation_Ed(animator);


            int countSkills = _skillsSettings.Length;
            if (_swapA != _swapB && _swapA >= 0 & _swapB >= 0 && _swapA < countSkills & _swapB < countSkills)
                (_skillsSettings[_swapA], _skillsSettings[_swapB]) = (_skillsSettings[_swapB], _skillsSettings[_swapA]);
            _swapA = _swapB = -1;

            _hitSfxNames = new(countSkills); _timings = new(countSkills);
            SkillSettings skillSettings; int index;

            for (index = 0; index < countSkills; index++)
            {
                skillSettings = _skillsSettings[index];

                _hitSfxNames[index] = skillSettings.hitSFXName_ed;
                _timings[index] = new(skillSettings.clipSettings_ed);

                if (animator[A_SKILLS[index]] != skillSettings.clipSettings_ed.clip)
                    animator[A_SKILLS[index]] = skillSettings.clipSettings_ed.clip;
            }

            for (; index < COUNT_SKILLS_MAX; index++)
                if (animator[A_SKILLS[index]].name != A_SKILLS[index])
                    animator[A_SKILLS[index]] = null;
        }
#endif
    }
}
