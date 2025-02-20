//Assets\Colonization\Editor\Actors\Fractions\Demons\Window\DemonsSettingsWindow.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.Actors;

namespace VurbiriEditor.Colonization.Actors
{
    using static CONST_EDITOR;

    public class DemonsSettingsWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Demons Settings", MENU = MENU_CH_PATH + "Actors/" + NAME;
        #endregion

        [SerializeField] private DemonsSettingsScriptable _demonsSettings;

        private static readonly Vector2 wndMinSize = new(650f, 800f);

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<DemonsSettingsWindow>(true, NAME).minSize = wndMinSize;
        }

        public void CreateGUI()
        {
            if (_demonsSettings == null)
            {
                Debug.LogWarning("Set DemonsSettingsScriptable");
                _demonsSettings = VurbiriEditor.Utility.FindAnyScriptable<DemonsSettingsScriptable>();
                if (_demonsSettings == null)
                    return;
            }

            rootVisualElement.Add(DemonsSettingsEditor.CreateCachedEditorAndBind(_demonsSettings));
        }

        private void OnDisable()
        {
            if (_demonsSettings != null)
                ActorUtility.OverrideClips(_demonsSettings.Settings);
        }
    }
}
