using UnityEngine;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public class Prices : ScriptableObject
    {
        [SerializeField] private CurrenciesLite _playersDefault;
        [SerializeField] private ReadOnlyMainCurrencies _roads;
        [SerializeField] private ReadOnlyMainCurrencies _wall;
        [SerializeField] private ReadOnlyIdArray<EdificeId, ReadOnlyMainCurrencies> _edifices;
        [SerializeField] private ReadOnlyIdArray<WarriorId, ReadOnlyMainCurrencies> _warriors;

        public CurrenciesLite HumanDefault { [Impl(256)] get => _playersDefault; }
        public ReadOnlyMainCurrencies Road { [Impl(256)] get => _roads; }
        public ReadOnlyMainCurrencies Wall { [Impl(256)] get => _wall; }
        public ReadOnlyIdArray<EdificeId, ReadOnlyMainCurrencies> Edifices { [Impl(256)] get => _edifices; }
        public ReadOnlyIdArray<WarriorId, ReadOnlyMainCurrencies> Warriors { [Impl(256)] get => _warriors; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _playersDefault ??= new();
            _roads ??= new();
            _wall ??= new();
            _edifices ??= new(() => new());
            _warriors ??= new(() => new());
        }

        public void InputAll_Ed()
        {
            MainCurrencies all = new() { _roads, _wall };
            for (int i = EdificeId.Shrine; i < EdificeId.Count; ++i)
                all.Add(_edifices[i]);
            for (int i = 0; i < WarriorId.Count; ++i)
                all.Add(_warriors[i]);

            Draw_Ed(all, " ALL ");
        }
        public void InputStart_Ed()
        {
            MainCurrencies start = new() { _roads * 3, _edifices[EdificeId.Camp], _warriors[WarriorId.Militia] };
            Draw_Ed(start, " START ");
        }

        public void InputRoads_Ed(int count) => Draw_Ed(_roads * count, " ROADS ");
        public void InputColonies_Ed(int count) => Draw_Ed(GetColoniesCost() * count, " COLONIES ");
        public void InputPortsOne_Ed(int count) => Draw_Ed(GetPortsOneCost() * count, " PORTS ONE ");
        public void InputPortsTwo_Ed(int count) => Draw_Ed(GetPortsTwoCost() * count, " PORTS TWO ");
        public void InputEdifices_Ed(int coloniesCount, int portsCount, int roadsCount)
        {
            MainCurrencies edifices = new() { GetColoniesCost() * coloniesCount, GetPortsTwoCost() * portsCount, _roads * roadsCount, };
            Draw_Ed(edifices, " EDIFICES ");
        }

        private void Draw_Ed(ReadOnlyMainCurrencies currencies, string caption = "===")
        {
            Debug.Log(string.Format("====================={0}=====================", caption));
            System.Text.StringBuilder sb = new();
            for (int i = 0; i < CurrencyId.MainCount; ++i)
            {
                sb.Append("[<b>"); sb.Append(CurrencyId.Names_Ed[i]); sb.Append("</b>: "); sb.Append(currencies[i].ToString()); sb.Append("] ");
            }
            sb.Append(" |<b>Amount: "); sb.Append(currencies.Amount.ToString()); sb.Append("</b>|");
            Debug.Log(sb.ToString());
        }

        private MainCurrencies GetColoniesCost() =>  new() { _edifices[EdificeId.Camp], _edifices[EdificeId.Town], _edifices[EdificeId.City], _wall };
        private MainCurrencies GetPortsOneCost() => new() { _edifices[EdificeId.PortOne], _edifices[EdificeId.LighthouseOne] };
        private MainCurrencies GetPortsTwoCost() => new() { _edifices[EdificeId.PortTwo], _edifices[EdificeId.LighthouseTwo] };
#endif
    }
}
