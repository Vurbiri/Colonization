using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public abstract class ActorSettingsScriptable<TId, TSettings> : ScriptableObjectDisposable where TId : ActorId<TId> where TSettings : ActorSettings
	{
        [SerializeField] private IdArray<TId, TSettings> _settings;

        public ReadOnlyArray<TSettings> Settings => _settings;

        public TSettings[] Init(out int force)
        {
            TSettings settings;
            int minForce = int.MaxValue, maxForce = int.MinValue;
            for (int i = 0; i < ActorId<TId>.Count; i++)
            {
                settings = _settings[i];
                settings.Init();
                minForce = Mathf.Min(minForce, settings.Force);
                maxForce = Mathf.Max(maxForce, settings.Force);
            }
            force = minForce + maxForce;

            return _settings.Values;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            for (int i = 0; i < ActorId<TId>.Count; i++)
                _settings[i].OnValidate();
        }

        public void UpdateName_Ed(string oldName, string newName)
        {
            bool isDirty = false;
            for (int i = 0; i < ActorId<TId>.Count; i++)
                isDirty |= _settings[i].UpdateName_Ed(oldName, newName);

            if(isDirty)
                UnityEditor.EditorUtility.SetDirty(this);
        }

        public void UpdateAnimation_Ed()
        {
            for (int i = 0; i < ActorId<TId>.Count; i++)
                _settings[i].UpdateAnimation_Ed();

            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }

        public void PrintForce_Ed()
        {
            Debug.Log("==== Actor Force ====");
            for (int i = 0; i < ActorId<TId>.Count; i++)
                _settings[i].PrintForce_Ed();
        }

        public void PrintProfit_Ed(int main, int adv)
        {
            Debug.Log("==== Actor Profit ====");
            for (int i = 0; i < ActorId<TId>.Count; i++)
                _settings[i].PrintProfit_Ed(main, adv);
        }

        public void SetSkills_Ed(ref string[][] names, ref int[][] values)
        {
            names = new string[ActorId<TId>.Count][];
            values = new int[ActorId<TId>.Count][];
            for (int i = 0; i < ActorId<TId>.Count; i++)
                (names[i], values[i]) = GetSkills(_settings[i].Skills.SkillSettings_Ed);

            static (string[], int[]) GetSkills(SkillSettings[] skillSettings)
            {
                int count = skillSettings.Length;
                string[] names = new string[count + 1];
                int[] values = new int[count + 1];

                names[0]  = "------------";
                values[0] = -1;

                for (int i = 0, j = 1; i < count; i++, j++)
                {
                    names[j]  = $"{skillSettings[i].GetName_Ed()} ({i.ToStr()})";
                    values[j] = i;
                }

                return (names, values);
            }
        }
#endif
    }
}
