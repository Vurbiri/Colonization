using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class RecruiterSettingsWindow : ASettingsWindow<RecruiterSettings>
    {
        private const string NAME = "Recruiter", MENU = MENU_CR_PATH + NAME;

        [MenuItem(MENU, false, MENU_AI_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<RecruiterSettingsWindow>(true, NAME);
        }
    }
}
