using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri;
using Vurbiri.Colonization.Actors;


namespace VurbiriEditor.Colonization.Actors
{
	public abstract class ActorsSettingsWindow<TScriptable, TId, TValue> : EditorWindow 
        where TScriptable : ActorSettingsScriptable<TId, TValue> 
        where TId : ActorId<TId> where TValue : ActorSettings
    {
        [SerializeField] private TScriptable _actorsSettings;

        public void CreateGUI()
        {
            EUtility.CheckScriptable<TScriptable>(ref _actorsSettings, typeof(TValue).Name, "Assets/Colonization/Settings/Characteristics");

            var root = CreateEditor(_actorsSettings);
            root.Q<Button>("Refresh").clicked += Refresh;
            root.Q<Button>("ApplyUp").clicked += Apply;
            root.Q<Button>("ApplyDown").clicked += Apply;

            rootVisualElement.Add(root);
        }

        protected abstract VisualElement CreateEditor(TScriptable settings);

        private void Refresh()
        {
            rootVisualElement.Clear();
            CreateGUI();
        }

        private void Apply()
        {
            if (_actorsSettings != null)
            {
                _actorsSettings.UpdateAnimation_Ed();
                Repaint();
            }
        }
    }
}
