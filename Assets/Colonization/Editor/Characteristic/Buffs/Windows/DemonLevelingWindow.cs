using UnityEditor;

namespace VurbiriEditor.Colonization
{
    sealed public class DemonLevelingWindow : ABuffsWindow
    {
        private const string NAME = "Demon Leveling", MENU = CONST_EDITOR.MENU_BUFFS_PATH + NAME;

        [MenuItem(MENU, false, 14)]
        private static void ShowWindow()
		{
			GetWindow<DemonLevelingWindow>(true, NAME).minSize = new(300f, 500f); ;
		}

        private void OnEnable()
        {
            base.Enable("DemonLevelingSettings", "Level Up", 30);
        }
        private void OnDisable()
        {
            base.Disable(false);
        }
    }
}
