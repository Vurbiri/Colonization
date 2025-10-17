using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class CasterSettingsWindow : ASettingsWindow<CasterSettings>
    {
        private const string NAME = "Caster", MENU = MENU_CR_PATH + NAME;

        [MenuItem(MENU, false, MENU_AI_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<CasterSettingsWindow>(true, NAME);
        }
    }
}
