using UnityEditor;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class MilitaryPerksWindow : APerksWindow<MilitaryPerksEditor>
    {
        private const string NAME = "Military", MENU = MENU_PERKS_PATH + NAME;

        [MenuItem(MENU, false, MENU_PERKS_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<MilitaryPerksWindow>(false, NAME).minSize = s_sizeWindow;
        }
    }
}
