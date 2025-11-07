using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
	public abstract class ActorsSettingsWindow<TScriptable, TId, TValue> : EditorWindow 
        where TScriptable : ActorSettingsScriptable<TId, TValue> 
        where TId : ActorId<TId> where TValue : ActorSettings
    {
        [SerializeField] protected TScriptable _actorsSettings;

        protected int _mainProfit, _advProfit;

        public void CreateGUI()
        {
            EUtility.CheckScriptable<TScriptable>(ref _actorsSettings, typeof(TValue).Name, "Assets/Colonization/Settings/Characteristics");

            var root = CreateEditor(_actorsSettings);
            root.Q<Button>("Force").clicked += PrintForce;
            root.Q<Button>("Profit").clicked += PrintProfit;
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

        private void PrintForce() => _actorsSettings?.PrintForce_Ed();
        private void PrintProfit() => _actorsSettings?.PrintProfit_Ed(_mainProfit, _advProfit);

        private void Apply()
        {
            if (_actorsSettings != null)
            {
                _actorsSettings.UpdateAnimation_Ed();
                Repaint();
            }
        }

        private void OnDestroy()
        {
            if (_actorsSettings != null)
                SkillDrawer.Update<TScriptable, TId, TValue>(_actorsSettings);
        }
    }
}
