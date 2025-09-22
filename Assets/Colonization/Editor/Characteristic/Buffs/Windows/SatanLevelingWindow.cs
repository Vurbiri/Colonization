using UnityEditor;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    sealed public class SatanLevelingWindow : ABuffsWindow
    {
        private const string NAME = "Satan Leveling", MENU = MENU_SATAN_PATH + NAME;

        [MenuItem(MENU, false, MENU_SATAN_ORDER)]
        private static void ShowWindow()
		{
			GetWindow<SatanLevelingWindow>(true, NAME).minSize = s_minSize;
		}

        private void OnEnable()
        {
            base.Enable(NAME, "Level Up", 35);
        }
        private void OnDisable()
        {
            base.Disable(false);
        }
    }
}
