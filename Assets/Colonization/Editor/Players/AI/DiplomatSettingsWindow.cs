using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class DiplomatSettingsWindow : ASettingsWindow<DiplomatSettings>
    {
        private const string NAME = "Diplomat", MENU = MENU_CR_PATH + NAME;

        [MenuItem(MENU, false, MENU_AI_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<DiplomatSettingsWindow>(true, NAME);
        }
    }
}
