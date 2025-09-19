using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class PersonSettingsWindow : ASettingsWindow<PersonSettings>
    {
        private const string NAME = "Person", MENU = MENU_CR_PATH + NAME;

        [MenuItem(MENU, false, MENU_CR_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<PersonSettingsWindow>(true, NAME);
        }
    }
}
