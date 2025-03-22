//Assets\Colonization\Editor\Characteristic\Abilities\Windows\PlayerAbilitiesWindow.cs
namespace VurbiriEditor.Colonization.Characteristics
{
    using UnityEditor;
    using UnityEngine;
    using Vurbiri;
    using Vurbiri.Colonization.Characteristics;
    using static CONST_EDITOR;

    public class HumanAbilitiesWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Human Abilities", MENU = MENU_CH_PATH + NAME;
        #endregion

        [SerializeField] private HumanAbilitiesScriptable _scriptable;

        [MenuItem(MENU, false, 21)]
        private static void ShowWindow()
        {
            GetWindow<HumanAbilitiesWindow>(true, NAME).minSize = new(225f, 300f);
        }

        public void CreateGUI()
        {
            if (_scriptable == null)
                _scriptable = EUtility.FindAnyScriptable<HumanAbilitiesScriptable>();
                          
            rootVisualElement.Add(HumanAbilitiesEditor.CreateCachedEditorAndBind(_scriptable));
        }
    }
}
