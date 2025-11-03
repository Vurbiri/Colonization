using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class WarriorAISettingsWindow : ASettingsWindow<WarriorAISettings>
    {
        private const string NAME = "Warriors", MENU = MENU_AA_PATH + NAME;

        [MenuItem(MENU, false, MENU_AI_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<WarriorAISettingsWindow>(true, NAME);
        }
    }
}
