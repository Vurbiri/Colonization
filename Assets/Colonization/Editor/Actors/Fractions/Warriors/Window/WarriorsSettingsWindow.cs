using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri.Colonization.Actors;

namespace VurbiriEditor.Colonization.Actors
{
    using static CONST_EDITOR;

    public class WarriorsSettingsWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Warriors Settings", MENU = MENU_ACTORS_PATH + NAME;
        #endregion

        [SerializeField] private WarriorsSettingsScriptable _warriorsSettings;

        [MenuItem(MENU, false, 10)]
        private static void ShowWindow()
        {
            GetWindow<WarriorsSettingsWindow>(true, NAME).minSize = new(650f, 800f);
        }

        public void CreateGUI()
        {
            if (_warriorsSettings == null)
            {
                Debug.LogWarning("Set WarriorsSettingsScriptable");
                _warriorsSettings = Vurbiri.EUtility.FindAnyScriptable<WarriorsSettingsScriptable>();
                if (_warriorsSettings == null)
                    return;
            }

            var root = WarriorsSettingsEditor.CreateCachedEditorAndBind(_warriorsSettings);
            root.Q<Button>("Apply").clicked += Apply;

            rootVisualElement.Add(root);
        }

        private void Apply()
        {
            if (_warriorsSettings != null)
                ActorUtility.OverrideClips(_warriorsSettings.Settings);
        }
    }
}
