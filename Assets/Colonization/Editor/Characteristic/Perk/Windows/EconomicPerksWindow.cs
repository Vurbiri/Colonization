using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    using static CONST_EDITOR;

    public class EconomicPerksWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Economic", MENU = MENU_PERKS_PATH + NAME;
        #endregion

        private static readonly Vector2 wndMinSize = new(375f, 800f);

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<EconomicPerksWindow>(false, NAME).minSize = wndMinSize;
        }

        public void CreateGUI()
        {
            EconomicPerksScriptable perks = Utility.FindAnyScriptable<EconomicPerksScriptable>();

            if (perks == null)
            {
                Debug.Log("Нет EconomicPerksScriptable");
                return;
            }

            rootVisualElement.Add(EconomicPerksEditor.CreateCachedEditorAndBind(perks));
        }
    }
}
