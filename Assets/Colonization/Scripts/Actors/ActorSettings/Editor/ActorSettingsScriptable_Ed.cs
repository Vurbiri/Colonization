#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract partial class ActorSettingsScriptable<TId, TSettings>
    {
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

        public void GetSelfSkills_Ed(ref string[][] names, ref int[][] values)
        {
            CreateArrays(ref names, ref values);
            for (int i = 0; i < ActorId<TId>.Count; ++i)
                _settings[i].Skills.GetSelfSkills_Ed(ref names[i], ref values[i]);
        }
        public void GetHeals_Ed(ref string[][] names, ref int[][] values)
        {
            CreateArrays(ref names, ref values);
            for (int i = 0; i < ActorId<TId>.Count; ++i)
                _settings[i].Skills.GetHeals_Ed(ref names[i], ref values[i]);
        }

        private void CreateArrays(ref string[][] names, ref int[][] values)
        {
            names = new string[ActorId<TId>.Count][];
            values = new int[ActorId<TId>.Count][];
        }
    }
}
#endif