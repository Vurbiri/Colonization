using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class DiplomacySettingsWindow : ASettingsWindow<DiplomacySettings>
	{
		#region Consts
		private const string NAME = "Diplomacy", MENU = MENU_GS_PATH + NAME;
    #endregion

        [MenuItem(MENU, false, MENU_GS_ORDER)]
		private static void ShowWindow()
		{
			GetWindow<DiplomacySettingsWindow>(true, NAME);
		}
    }
}
