using UnityEditor;
using Vurbiri.Collections;
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

        private void OnValidate()
        {
            if (_settings.economicKey == null || !IsValid(_settings.economicKey))
                _settings.economicKey = new(EconomicSpellId.Names_Ed);

            if (_settings.militaryKey == null || !IsValid(_settings.militaryKey))
                _settings.militaryKey = new(MilitarySpellId.Names_Ed);

            static bool IsValid(ReadOnlyArray<string> keys)
            {
                for (int i = 0; i < keys.Count; ++i)
                    if (string.IsNullOrEmpty(keys[i]))
                        return false;
                return true;
            }
        }

    }
}