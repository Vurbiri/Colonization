using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class ScoreSettingsWindow : ASettingsWindow<ScoreSettings>
    {
        private const string NAME = "Score", MENU = MENU_GS_PATH + NAME;

        [MenuItem(MENU, false, MENU_GS_ORDER)]
		private static void ShowWindow()
		{
			GetWindow<ScoreSettingsWindow>(true, NAME);
		}
	}
}
