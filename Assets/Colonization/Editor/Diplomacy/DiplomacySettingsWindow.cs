//Assets\Colonization\Editor\Diplomacy\DiplomacySettingsWindow.cs
using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class DiplomacySettingsWindow : ASettingsWindow<ScoreSettings>
	{
		#region Consts
		private const string NAME = "Diplomacy", MENU = MENU_PATH + NAME;
    #endregion

        protected override string Caption => "Diplomacy Settings";

        [MenuItem(MENU, false, 30)]
		private static void ShowWindow()
		{
			GetWindow<DiplomacySettingsWindow>(true, NAME);
		}
    }
}
