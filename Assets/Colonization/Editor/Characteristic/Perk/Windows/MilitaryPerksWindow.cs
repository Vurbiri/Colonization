using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization.Characteristics;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization.Characteristics
{
    public class MilitaryPerksWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Military", MENU = MENU_PERKS_PATH + NAME;
        #endregion

        [SerializeField] private PerksScriptable _perks;

        private Editor _editor;

        [MenuItem(MENU, false, MENU_PERKS_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<MilitaryPerksWindow>(false, NAME).minSize = new(375f, 800f);
        }

        public void CreateGUI()
        {
            if (_perks == null)
                _perks = EUtility.FindAnyScriptable<PerksScriptable>();

            rootVisualElement.Add(MilitaryPerksEditor.CreateEditorAndBind(_perks, out _editor));
        }

        private void OnDisable()
        {
            DestroyImmediate(_editor);
        }
    }
}
