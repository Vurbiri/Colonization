using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class ChaosSettingsWindow : ASettingsWindow<ChaosSettings>
    {
        private const string NAME = "Chaos", MENU = MENU_GS_PATH + NAME;

        [MenuItem(MENU, false, MENU_GS_ORDER)]
        private static void ShowWindow()
		{
			GetWindow<ChaosSettingsWindow>(true, NAME);
		}
	}
}