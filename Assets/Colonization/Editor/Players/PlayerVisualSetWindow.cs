//Assets\Colonization\Editor\Players\PlayerVisualSetWindow.cs
using UnityEditor;
using UnityEngine;

namespace VurbiriEditor.Colonization
{
    using static CONST_EDITOR;

    public class PlayerVisualSetWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Player Visual", MENU = MENU_UI_PATH + NAME;
        #endregion

        [SerializeField] private PlayerVisualSetScriptable _visualSet;

        private static readonly Vector2 wndMinSize = new(350f, 300f);

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<PlayerVisualSetWindow>(true, NAME).minSize = wndMinSize;
        }

        
        public void CreateGUI()
        {
            rootVisualElement.Add(PlayerVisualSetEditor.CreateCachedEditorAndBind(_visualSet));
        }

    }
}
