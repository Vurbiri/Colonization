using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class AIGiftSettingsWindow : ASettingsWindow<AIGiftSettings>
    {
        private const string NAME = "Gift", MENU = MENU_AI_PATH + NAME;

        [MenuItem(MENU, false, MENU_AI_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<AIGiftSettingsWindow>(true, NAME);
        }
    }
}
