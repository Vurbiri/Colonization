using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class HexWeightWindow : ASettingsWindow<HexWeight>
    {
        private const string NAME = "HexWeight", MENU = MENU_AI_PATH + NAME;

        [MenuItem(MENU, false, MENU_AI_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<HexWeightWindow>(true, NAME);
        }

        private void OnValidate()
        {
            _settings.OnValidate();
        }
    }
}
