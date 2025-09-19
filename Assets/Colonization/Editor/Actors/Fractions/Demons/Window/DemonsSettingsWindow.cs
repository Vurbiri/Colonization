using UnityEditor;
using UnityEngine.UIElements;
using Vurbiri.Colonization.Actors;

namespace VurbiriEditor.Colonization.Actors
{
    using static CONST_EDITOR;

    sealed public class DemonsSettingsWindow : ActorsSettingsWindow<DemonsSettingsScriptable, DemonId, DemonSettings>
    {
        private const string NAME = "Demons Settings", MENU = MENU_AC_PATH + NAME;

        [MenuItem(MENU, false, MENU_AC_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<DemonsSettingsWindow>(true, NAME).minSize = new(650f, 800f);
        }

        protected override VisualElement CreateEditor(DemonsSettingsScriptable settings) => DemonsSettingsEditor.CreateCachedEditorAndBind(settings);
    }
}
