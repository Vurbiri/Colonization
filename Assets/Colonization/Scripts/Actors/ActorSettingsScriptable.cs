using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public abstract class ActorSettingsScriptable<TId, TSettings> : ScriptableObjectDisposable where TId : ActorId<TId> where TSettings : ActorSettings
	{
        [SerializeField] private IdArray<TId, TSettings> _settings;

        public ReadOnlyIdArray<TId, TSettings> Settings => _settings;

        public TSettings[] Init()
        {
            for (int i = 0; i < ActorId<TId>.Count; i++)
                _settings[i].Init();

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
#endif
    }
}
