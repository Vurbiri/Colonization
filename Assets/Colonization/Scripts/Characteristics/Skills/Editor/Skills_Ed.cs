#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization
{
    public partial class Skills
    {
        private static readonly string[] A_SKILLS = { "A_Skill_0", "A_Skill_1", "A_Skill_2", "A_Skill_3" };

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

        public void GetSelfSkills_Ed(ref string[] names, ref int[] values) => GetSkills_Ed(ref names, ref values, (s) => s.Target == TargetOfSkill.Self);
        public void GetHeals_Ed(ref string[] names, ref int[] values) => GetSkills_Ed(ref names, ref values, (s) => s.IsHeal_Ed(), false);

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

        private void GetSkills_Ed(ref string[] names, ref int[] values, Func<SkillSettings, bool> valid, bool isNone = true, string keySpec = null)
        {
            int count = _skillsSettings.Length;
            List<string> listNames = new(count);
            List<int> listValues = new(count);

            if(isNone)
                AddEmpty(listNames, listValues);

            for (int i = 0; i < count; ++i)
            {
                if (valid(_skillsSettings[i]))
                {
                    listNames.Add($"{_skillsSettings[i].GetName_Ed()} ({i.ToStr()})");
                    listValues.Add(i);
                }
            }

            if(!string.IsNullOrEmpty(keySpec))
            {
                listNames.Add(GetName_Ed(keySpec));
                listValues.Add(CONST.SPEC_SKILL_ID);
            }

            if (listNames.Count == 0)
                AddEmpty(listNames, listValues);

            names = listNames.ToArray(); values = listValues.ToArray();

            // ========= Local========
            static void AddEmpty(List<string> listNames, List<int> listValues)
            {
                listNames.Add("--------------");
                listValues.Add(-1);
            }
            static string GetName_Ed(string key)
            {
                return Vurbiri.International.Localization.ForEditor(CONST_UI.FILE).GetText(CONST_UI.FILE, key);
            }
        }
    }
}
#endif