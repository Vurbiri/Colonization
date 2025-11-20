#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public partial class Skills
    {
        private static readonly string[] A_SKILLS = { "A_Skill_0", "A_Skill_1", "A_Skill_2", "A_Skill_3" };
        private static readonly Func<SkillSettings, bool>[] s_valid = { SkillSettings.IsAttack_Ed, SkillSettings.IsSelf_Ed, SkillSettings.IsBuff_Ed, SkillSettings.IsDebuff_Ed };

        [SerializeField] private int _swapA = -1;
        [SerializeField] private int _swapB = -1;

        public void OnValidate(int type, int id)
        {
            _specSkillSettings.OnValidate(type, id);

            if (_skillsSettings.Length > CONST.MAIN_SKILLS_COUNT)
                Array.Resize(ref _skillsSettings, CONST.MAIN_SKILLS_COUNT);

            for (int i = 0; i < _skillsSettings.Length; ++i)
                _skillsSettings[i].typeActor_ed = type;
        }

        public void GetDefence_Ed(ref string[] names, ref int[] values)
        {
            int count = _skillsSettings.Length;
            List<string> listNames = new(count);
            List<int> listValues = new(count);

            AddEmpty(listNames, listValues);

            for (int i = 0; i < count; ++i)
            {
                if (SkillSettings.IsSelf_Ed(_skillsSettings[i]))
                {
                    listNames.Add(GetSkillName(i));
                    listValues.Add(i);
                }
            }

            names = listNames.ToArray(); values = listValues.ToArray();
        }

        public void GetSkills_Ed(int skillType, ref GUIContent[] labels, ref int[] values)
        {
            var valid = s_valid[skillType];
            int count = _skillsSettings.Length;
            labels = new GUIContent[count];
            List<int> listValues = new(count);

            for (int i = 0; i < count; ++i)
            {
                if (valid(_skillsSettings[i]))
                {
                    labels[i] = new(GetSkillName(i));
                    listValues.Add(i);
                }
            }

            values = listValues.ToArray();
        }

        public (string name, int value) GetHeals_Ed()
        {
            int count = _skillsSettings.Length;
            var output = GetEmptySkillName();

            for (int i = 0; i < count; ++i)
            {
                if (_skillsSettings[i].IsHeal_Ed())
                {
                    output.name = GetSkillName(i);
                    output.value = i;
                    break;
                }
            }

            return output;
        }

        public bool UpdateSFXName_Ed(string oldName, string newName)
        {
            bool changed = false;
            for (int i = 0; i < _skillsSettings.Length; i++)
            {
                if (_skillsSettings[i].hitSFXName_ed.Equals(oldName))
                {
                    changed = true;
                    _skillsSettings[i].hitSFXName_ed = new(newName);
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

            for (; index < CONST.MAIN_SKILLS_COUNT; index++)
                if (animator[A_SKILLS[index]].name != A_SKILLS[index])
                    animator[A_SKILLS[index]] = null;
        }

        private static void AddEmpty(List<string> listNames, List<int> listValues)
        {
            var (name, value) = GetEmptySkillName();
            listNames.Add(name);
            listValues.Add(value);
        }

        private static (string name, int value) GetEmptySkillName() => ("----------------", -1);
        private string GetSkillName(int i) => $"{_skillsSettings[i].GetName_Ed()} [ID {i.ToStr()}]";
    }
}
#endif