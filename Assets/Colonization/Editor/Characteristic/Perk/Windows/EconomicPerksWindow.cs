//Assets\Colonization\Editor\Characteristic\Perk\Windows\EconomicPerksWindow.cs
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization.Characteristics;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization.Characteristics
{
    public class EconomicPerksWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Economic", MENU = MENU_PERKS_PATH + NAME;
        #endregion

        [SerializeField] private EconomicPerksScriptable _perks;

        private Editor _editor;

        [MenuItem(MENU, false, 14)]
        private static void ShowWindow()
        {
            GetWindow<EconomicPerksWindow>(false, NAME).minSize = new(375f, 800f);
        }

        public void CreateGUI()
        {
            if (_perks == null)
                _perks = EUtility.FindAnyScriptable<EconomicPerksScriptable>();

            rootVisualElement.Add(EconomicPerksEditor.CreateEditorAndBind(_perks, out _editor));
        }

        private void OnDisable()
        {
            DestroyImmediate(_editor);
        }
    }
}
