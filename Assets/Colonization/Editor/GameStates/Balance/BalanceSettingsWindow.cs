using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class BalanceSettingsWindow : ASettingsWindow<BalanceSettings>
    {
		#region Consts
		private const string NAME = "Balance", MENU = MENU_GS_PATH + NAME;
		#endregion
		
		[MenuItem(MENU)]
		private static void ShowWindow()
		{
			GetWindow<BalanceSettingsWindow>(true, NAME);
		}
	}
}