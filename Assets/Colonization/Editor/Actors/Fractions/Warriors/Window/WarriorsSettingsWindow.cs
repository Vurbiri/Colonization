using UnityEditor;
using UnityEngine.UIElements;
using Vurbiri.Colonization.Actors;

namespace VurbiriEditor.Colonization.Actors
{
    using static CONST_EDITOR;

    sealed public class WarriorsSettingsWindow : ActorsSettingsWindow<WarriorsSettingsScriptable, WarriorId, WarriorSettings>
    {
        private const string NAME = "Warriors Settings", MENU = MENU_AC_PATH + NAME;

        [MenuItem(MENU, false, 10)]
        private static void ShowWindow()
        {
            GetWindow<WarriorsSettingsWindow>(true, NAME).minSize = new(650f, 800f);
        }

        protected override VisualElement CreateEditor(WarriorsSettingsScriptable settings) => WarriorsSettingsEditor.CreateCachedEditorAndBind(settings);
    }
}
