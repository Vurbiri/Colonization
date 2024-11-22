//Assets\Colonization\Editor\Characteristic\Perk\Windows\MilitaryPerksWindow.cs
namespace VurbiriEditor.Colonization.Characteristics
{
    using UnityEditor;
    using UnityEngine;
    using Vurbiri.Colonization.Characteristics;
    using static CONST_EDITOR;

    public class MilitaryPerksWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Military", MENU = MENU_PERKS_PATH + NAME;
        #endregion

        private static readonly Vector2 wndMinSize = new(375f, 800f);

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<MilitaryPerksWindow>(false, NAME).minSize = wndMinSize;
        }

        public void CreateGUI()
        {
            MilitaryPerksScriptable perks = Utility.FindAnyScriptable<MilitaryPerksScriptable>();

            if (perks == null)
            {
                Debug.Log("��� MilitaryPerksScriptable");
                return;
            }

            rootVisualElement.Add(MilitaryPerksEditor.CreateCachedEditorAndBind(perks));
        }
    }
}
