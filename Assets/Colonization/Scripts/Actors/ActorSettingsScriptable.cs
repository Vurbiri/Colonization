using UnityEditor;
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
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
