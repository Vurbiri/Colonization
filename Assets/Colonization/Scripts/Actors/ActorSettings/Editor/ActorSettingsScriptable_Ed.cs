#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract partial class ActorSettingsScriptable<TId, TSettings>
    {
        private delegate void InitArrays(Skills skills, ref string[] names, ref int[] values);


        public int TypeId_Ed => _settings[0].TypeId;
        
        private void OnValidate()
        {
            for (int i = 0; i < ActorId<TId>.Count; ++i)
                _settings[i].OnValidate();
        }

        public void UpdateSFXName_Ed(string oldName, string newName)
        {
            bool isDirty = false;
            for (int i = 0; i < ActorId<TId>.Count; ++i)
                isDirty |= _settings[i].UpdateSFXName_Ed(oldName, newName);

            if (isDirty)
                EditorUtility.SetDirty(this);
        }

        public void UpdateAnimation_Ed()
        {
            for (int i = 0; i < ActorId<TId>.Count; ++i)
                _settings[i].UpdateAnimation_Ed();

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public void PrintForce_Ed()
        {
            Debug.Log("==== Actor Force ====");
            for (int i = 0; i < ActorId<TId>.Count; ++i)
                _settings[i].PrintForce_Ed();
            Debug.Log("=====================");
        }

        public void PrintProfit_Ed(int main, int adv)
        {
            Debug.Log("==== Actor Profit ====");
            for (int i = 0; i < ActorId<TId>.Count; ++i)
                _settings[i].PrintProfit_Ed(main, adv);
            Debug.Log("======================");
        }

        public void GetDefenseSkills_Ed(ref string[][] names, ref int[][] values) => GetValues_Ed(ref names, ref values, Skills.GetDefence_Ed);
        public void GetSelfSkills_Ed(ref string[][] names, ref int[][] values) => GetValues_Ed(ref names, ref values, Skills.GetSelf_Ed);
        public void GetHeals_Ed(ref string[] names, ref int[] values)
        {
            names = new string[ActorId<TId>.Count];
            values = new int[ActorId<TId>.Count];
            for (int i = 0; i < ActorId<TId>.Count; ++i)
                (names[i], values[i]) = _settings[i].Skills.GetHeals_Ed();
        }

        private void GetValues_Ed(ref string[][] names, ref int[][] values, InitArrays initArrays)
        {
            names = new string[ActorId<TId>.Count][];
            values = new int[ActorId<TId>.Count][];
            for (int i = 0; i < ActorId<TId>.Count; ++i)
                initArrays(_settings[i].Skills, ref names[i], ref values[i]);
        }
    }
}
#endif