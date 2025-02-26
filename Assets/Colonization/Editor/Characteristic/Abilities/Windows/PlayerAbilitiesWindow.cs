//Assets\Colonization\Editor\Characteristic\Abilities\Windows\PlayerAbilitiesWindow.cs
namespace VurbiriEditor.Colonization.Characteristics
{
    using UnityEditor;
    using UnityEngine;
    using Vurbiri;
    using Vurbiri.Colonization.Characteristics;
    using static CONST_EDITOR;

    public class PlayerAbilitiesWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Player Abilities", MENU = MENU_CH_PATH + NAME;
        #endregion

        public static readonly Vector2 wndMinSize = new(225f, 300f);

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<PlayerAbilitiesWindow>(true, NAME).minSize = wndMinSize;
        }

        public void CreateGUI()
        {
            PlayerAbilitiesScriptable abilities = EUtility.FindAnyScriptable<PlayerAbilitiesScriptable>();

            if (abilities == null)
            {
                Debug.Log("��� PlayerAbilitiesScriptable");
                return;
            }
            
            rootVisualElement.Add(PlayerAbilitiesScriptableEditor.CreateCachedEditorAndBind(abilities));
        }
    }
}
