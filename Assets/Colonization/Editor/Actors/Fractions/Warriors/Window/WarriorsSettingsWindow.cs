using UnityEditor;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    using static CONST_EDITOR;

    sealed public class WarriorsSettingsWindow : ActorsSettingsWindow<WarriorsSettingsScriptable, WarriorId, WarriorSettings>
    {
        private const string NAME = "Warriors Settings", MENU = MENU_AC_PATH + NAME;

        [MenuItem(MENU, false, MENU_AC_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<WarriorsSettingsWindow>(true, NAME).minSize = new(650f, 800f);
        }

        protected override VisualElement CreateEditor(WarriorsSettingsScriptable settings) => WarriorsSettingsEditor.CreateCachedEditorAndBind(settings);
    }
}
