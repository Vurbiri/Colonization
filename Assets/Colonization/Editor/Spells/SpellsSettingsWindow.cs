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

        private void OnValidate()
        {
            if (_settings.economicKey != null)
            {
                for (int i = 0; i < EconomicSpellId.Count; i++)
                    if (string.IsNullOrEmpty(_settings.economicKey[i]))
                        _settings.economicKey[i] = EconomicSpellId.Names_Ed[i];
            }
            else
            {
                _settings.economicKey = new(EconomicSpellId.Names_Ed);
            }

            if (_settings.militaryKey != null)
            {
                for (int i = 0; i < MilitarySpellId.Count; i++)
                    if (string.IsNullOrEmpty(_settings.militaryKey[i]))
                        _settings.militaryKey[i] = MilitarySpellId.Names_Ed[i];
            }
            else
            {
                _settings.militaryKey = new(MilitarySpellId.Names_Ed);
            }
        }

    }
}