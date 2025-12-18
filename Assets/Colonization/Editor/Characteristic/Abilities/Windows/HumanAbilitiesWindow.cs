using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{

    public class HumanAbilitiesWindow : EditorWindow
    {
        private const string NAME = "Human Abilities", MENU = MENU_CH_PATH + NAME;

        [SerializeField] private HumanAbilitiesScriptable _scriptable;

        [MenuItem(MENU, false, MENU_CH_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<HumanAbilitiesWindow>(true, NAME).minSize = new(225f, 570f);
        }

        public void CreateGUI()
        {
            if (_scriptable == null)
                _scriptable = EUtility.FindAnyScriptable<HumanAbilitiesScriptable>();
                          
            rootVisualElement.Add(HumanAbilitiesEditor.CreateCachedEditorAndBind(_scriptable));
        }
    }
}
