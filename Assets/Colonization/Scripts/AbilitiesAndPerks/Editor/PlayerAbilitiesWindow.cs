using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    using static CONST_EDITOR;

    public class PlayerAbilitiesWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Player abilities", MENU = MENU_PA_PATH + NAME;
        #endregion

        [SerializeField] private PlayerAbilitiesScriptable _abilities;

        public static readonly Vector2 wndMinSize = new(225f, 300f);

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<PlayerAbilitiesWindow>(true, NAME).minSize = wndMinSize;
        }

        public void CreateGUI()
        {
            if(_abilities == null)
            {
                Debug.Log("Нет PlayerAbilitiesScriptable");
                return;
            }
            
            rootVisualElement.Add(PlayerAbilitiesScriptableEditor.GetVisualElement(new(_abilities)));
        }
    }
}
