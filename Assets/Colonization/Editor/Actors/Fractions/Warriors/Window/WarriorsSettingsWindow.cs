//Assets\Colonization\Editor\Actors\Fractions\Warriors\Window\WarriorsSettingsWindow.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.Actors;

namespace VurbiriEditor.Colonization.Actors
{
    using static CONST_EDITOR;

    public class WarriorsSettingsWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Warriors Settings", MENU = MENU_CH_PATH + "Actors/" + NAME;
        #endregion

        [SerializeField] private WarriorsSettingsScriptable _warriorsSettings;

        private static readonly Vector2 wndMinSize = new(650f, 800f);

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<WarriorsSettingsWindow>(true, NAME).minSize = wndMinSize;
        }

        public void CreateGUI()
        {
            if (_warriorsSettings == null)
            {
                Debug.LogWarning("Set WarriorsSettingsScriptable");
                _warriorsSettings = VurbiriEditor.Utility.FindAnyScriptable<WarriorsSettingsScriptable>();
                if (_warriorsSettings == null)
                    return;
            }

            rootVisualElement.Add(WarriorsSettingsEditor.CreateCachedEditorAndBind(_warriorsSettings));
        }

        private void OnDisable()
        {
            if (_warriorsSettings != null)
                ActorUtility.OverrideClips(_warriorsSettings.Settings);
        }
    }
}
