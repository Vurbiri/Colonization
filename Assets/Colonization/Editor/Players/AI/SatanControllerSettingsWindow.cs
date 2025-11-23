using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class SatanControllerSettingsWindow : ASettingsWindow<SatanControllerSettings>
    {
        private const string NAME = "SatanController", MENU = MENU_AI_PATH + NAME;

        [MenuItem(MENU, false, MENU_AI_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<SatanControllerSettingsWindow>(true, NAME);
        }
    }
}
