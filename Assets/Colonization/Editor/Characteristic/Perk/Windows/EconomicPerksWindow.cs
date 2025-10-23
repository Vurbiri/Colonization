using UnityEditor;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class EconomicPerksWindow : APerksWindow<EconomicPerksEditor>
    {
        private const string NAME = "Economic", MENU = MENU_PERKS_PATH + NAME;

        [MenuItem(MENU, false, MENU_PERKS_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<EconomicPerksWindow>(false, NAME).minSize = s_sizeWindow;
        }
    }
}
