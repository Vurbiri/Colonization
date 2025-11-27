using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    using static CONST_EDITOR;

    public class PricesWindow : EditorWindow
    {
        private const string NAME = "Prices", MENU = MENU_PATH + NAME;

        [SerializeField] private Prices _prices;
        [SerializeField] private HumanAbilitiesScriptable _abilities;
        [SerializeField] private PerksScriptable _perks;

        private int _roadsCount;

        [MenuItem(MENU, false, 30)]
        private static void ShowWindow()
        {
            GetWindow<PricesWindow>(false, NAME).minSize = new(325f, 400f);
        }

        public void OnEnable()
        {
            var economy = _perks[EconomicPerksId.Type];
            _roadsCount = _abilities[HumanAbilityId.MaxRoad];
            for (int i = EconomicPerksId.MaxRoad_1; i <= EconomicPerksId.MaxRoad_5; ++i)
                _roadsCount += economy[i].Value;

        }

        public void CreateGUI()
        {
            EUtility.CheckScriptable(ref _prices, "Prices", "Assets/Colonization/Settings");
            
            var element = PricesEditor.CreateCachedEditorAndBind(_prices);

            element.Q<Button>("BAll").clicked += _prices.InputAll_Ed;
            element.Q<Button>("BStart").clicked += _prices.InputStart_Ed;
            element.Q<Button>("BColonies").clicked += () => _prices.InputColonies_Ed(_abilities[HumanAbilityId.MaxColony] + 2);
            element.Q<Button>("BPorts").clicked += () => _prices.InputPortsTwo_Ed(_abilities[HumanAbilityId.MaxPort] + 1);
            element.Q<Button>("BRoads").clicked += () => _prices.InputRoads_Ed(_roadsCount);
            element.Q<Button>("BEdifices").clicked += () => _prices.InputEdifices_Ed(_abilities[HumanAbilityId.MaxColony] + 2, _abilities[HumanAbilityId.MaxPort] + 1, _roadsCount);

            rootVisualElement.Add(element);
        }
       
    }
}
