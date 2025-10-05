using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class BuilderSettingsWindow : ASettingsWindow<BuilderSettings>
    {
        private const string NAME = "Builder", MENU = MENU_CR_PATH + NAME;

        [MenuItem(MENU, false, MENU_AI_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<BuilderSettingsWindow>(true, NAME);
        }
    }
}
