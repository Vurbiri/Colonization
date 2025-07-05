using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class SpellsSettingsWindow : ASettingsWindow<SpellsSettings>
    {
		#region Consts
		private const string NAME = "Spells Settings", MENU = MENU_PATH + NAME;
        #endregion

        [MenuItem(MENU, false, 30)]
        private static void ShowWindow()
        {
            GetWindow<SpellsSettingsWindow>(true, NAME);
        }
    }
}