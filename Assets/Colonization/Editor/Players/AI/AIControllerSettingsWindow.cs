using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class AIControllerSettingsWindow : ASettingsWindow<AIControllerSettings>
    {
        private const string NAME = "AIController", MENU = MENU_AI_PATH + NAME;

        [MenuItem(MENU, false, MENU_AI_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<AIControllerSettingsWindow>(true, NAME);
        }
    }
}
