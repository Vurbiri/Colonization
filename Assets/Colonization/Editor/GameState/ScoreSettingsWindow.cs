using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class ScoreSettingsWindow : ASettingsWindow<ScoreSettings>
    {
		#region Consts
		private const string NAME = "Score", MENU = MENU_GS_PATH + NAME;
        #endregion

        [MenuItem(MENU, false, 30)]
		private static void ShowWindow()
		{
			GetWindow<ScoreSettingsWindow>(true, NAME);
		}
	}
}
