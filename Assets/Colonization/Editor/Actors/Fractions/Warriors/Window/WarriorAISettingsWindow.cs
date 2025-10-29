using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class WarriorAISettingsWindow : ASettingsWindow<WarriorAISettings>
    {
        private const string NAME = "WarriorAI", MENU = MENU_AI_PATH + NAME;

        [MenuItem(MENU, false, MENU_AI_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<WarriorAISettingsWindow>(true, NAME);
        }
    }
}
