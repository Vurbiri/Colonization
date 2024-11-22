//Assets\Colonization\Editor\Actors\Warriors\Window\WarriorsSettingsWindow.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.Actors;

namespace VurbiriEditor.Colonization.Actors
{
    using static CONST_EDITOR;

    public class WarriorsSettingsWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Warriors Settings", MENU = MENU_CH_PATH + NAME;
        #endregion

        [SerializeField] private WarriorsSettingsScriptable _warriorsSettings;

        private static readonly Vector2 wndMinSize = new(475f, 800f);

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<WarriorsSettingsWindow>(true, NAME).minSize = wndMinSize;
        }

        public void CreateGUI()
        {
            if (_warriorsSettings == null)
            {
                Debug.Log("��� WarriorsSettingsScriptable");
                return;
            }

            rootVisualElement.Add(WarriorsSettingsEditor.CreateCachedEditorAndBind(_warriorsSettings));
        }
    }
}
