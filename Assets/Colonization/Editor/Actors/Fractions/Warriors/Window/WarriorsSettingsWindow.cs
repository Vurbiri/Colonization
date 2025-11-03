using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    using static CONST_EDITOR;

    sealed public class WarriorsSettingsWindow : ActorsSettingsWindow<WarriorsSettingsScriptable, WarriorId, WarriorSettings>
    {
        private const string NAME = "Warriors Settings", MENU = MENU_AC_PATH + NAME;

        [SerializeField] private PerksScriptable _perks;

        [MenuItem(MENU, false, MENU_AC_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<WarriorsSettingsWindow>(true, NAME).minSize = new(650f, 800f);
        }

        private void OnEnable()
        {
            if (_perks != null)
            {
                var mil = _perks[AbilityTypeId.Military];
                _mainProfit = 100 + mil[MilitaryPerksId.ProfitMain_1].Value + mil[MilitaryPerksId.ProfitMain_2].Value;
                _advProfit = 100 + mil[MilitaryPerksId.ProfitAdv_1].Value + mil[MilitaryPerksId.ProfitAdv_2].Value;
            }
        }

        protected override VisualElement CreateEditor(WarriorsSettingsScriptable settings) => WarriorsSettingsEditor.CreateCachedEditorAndBind(settings);

    }
}
