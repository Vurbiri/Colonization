using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class AISettingsWindow : ASettingsWindow<AISettings>
    {
        private const string NAME = "AI", MENU = MENU_CR_PATH + NAME;

        [MenuItem(MENU, false, MENU_CR_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<AISettingsWindow>(true, NAME);
        }
    }
}
