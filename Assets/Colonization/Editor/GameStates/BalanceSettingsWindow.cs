using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class BalanceSettingsWindow : ASettingsWindow<BalanceSettings>
    {
        private const string NAME = "Balance", MENU = MENU_GS_PATH + NAME;

        [MenuItem(MENU, false, MENU_GS_ORDER)]
        private static void ShowWindow()
		{
			GetWindow<BalanceSettingsWindow>(true, NAME);
		}
	}
}