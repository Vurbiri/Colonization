using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    using static CONST_EDITOR;

    public class PlayerStatesWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Player states", MENU = MENU_PA_PATH + NAME;
        #endregion

        [SerializeField] private PlayerStatesScriptable _states;

        public static readonly Vector2 wndMinSize = new(225f, 300f);

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<PlayerStatesWindow>(true, NAME).minSize = wndMinSize;
        }

        public void CreateGUI()
        {
            if(_states == null)
            {
                Debug.Log("Нет PlayerStatesScriptable");
                return;
            }
            
            rootVisualElement.Add(PlayerStatesScriptableEditor.GetVisualElement(new(_states)));
        }
    }
}
