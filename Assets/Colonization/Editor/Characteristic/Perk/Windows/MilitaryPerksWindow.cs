//Assets\Colonization\Editor\Characteristic\Perk\Windows\MilitaryPerksWindow.cs
namespace VurbiriEditor.Colonization.Characteristics
{
    using UnityEditor;
    using UnityEngine;
    using Vurbiri;
    using Vurbiri.Colonization.Characteristics;
    using static CONST_EDITOR;

    public class MilitaryPerksWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Military", MENU = MENU_PERKS_PATH + NAME;
        #endregion

        [SerializeField] private MilitaryPerksScriptable _perks;

        private Editor _editor;

        [MenuItem(MENU, false, 15)]
        private static void ShowWindow()
        {
            GetWindow<MilitaryPerksWindow>(false, NAME).minSize = new(375f, 800f);
        }

        public void CreateGUI()
        {
            if (_perks == null)
                _perks = EUtility.FindAnyScriptable<MilitaryPerksScriptable>();

            rootVisualElement.Add(MilitaryPerksEditor.CreateEditorAndBind(_perks, out _editor));
        }

        private void OnDisable()
        {
            DestroyImmediate(_editor);
        }
    }
}
