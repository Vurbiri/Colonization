using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    using static CONST_EDITOR;

    sealed public class DemonsSettingsWindow : ActorsSettingsWindow<DemonsSettingsScriptable, DemonId, DemonSettings>
    {
        private const string NAME = "Demons Settings", MENU = MENU_AC_PATH + NAME;

        [SerializeField] private BuffsScriptable _perks;

        

        [MenuItem(MENU, false, MENU_AC_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<DemonsSettingsWindow>(true, NAME).minSize = new(650f, 800f);
        }

        private void OnEnable()
        {
            if (_perks != null)
            {
                int max = _perks.MaxLevel;
                var settings = _perks.Settings;
                BuffSettings buff;
                for ( int i = 0; i < settings.Count; i++ )
                {
                    buff = settings[i];
                    if (buff.targetAbility == ActorAbilityId.ProfitMain)
                        _mainProfit = 100 + (buff.value * max / buff.advance);
                    if (buff.targetAbility == ActorAbilityId.ProfitAdv)
                        _advProfit = 100 + (buff.value * max / buff.advance);
                }
            }
        }

        protected override VisualElement CreateEditor(DemonsSettingsScriptable settings) => DemonsSettingsEditor.CreateCachedEditorAndBind(settings);
    }
}
