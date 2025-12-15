using UnityEngine;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public class Prices : ScriptableObject
    {
        [SerializeField] private StartCurrencies _playersDefault;
        [SerializeField] private ReadOnlyLiteCurrencies _roads;
        [SerializeField] private ReadOnlyLiteCurrencies _wall;
        [SerializeField] private ReadOnlyIdArray<EdificeId, ReadOnlyLiteCurrencies> _edifices;
        [SerializeField] private ReadOnlyIdArray<WarriorId, ReadOnlyLiteCurrencies> _warriors;

        public StartCurrencies HumanDefault { [Impl(256)] get => _playersDefault; }
        public ReadOnlyLiteCurrencies Road { [Impl(256)] get => _roads; }
        public ReadOnlyLiteCurrencies Wall { [Impl(256)] get => _wall; }
        public ReadOnlyIdArray<EdificeId, ReadOnlyLiteCurrencies> Edifices { [Impl(256)] get => _edifices; }
        public ReadOnlyIdArray<WarriorId, ReadOnlyLiteCurrencies> Warriors { [Impl(256)] get => _warriors; }

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
            LiteCurrencies all = new() { _roads, _wall };
            for (int i = EdificeId.Shrine; i < EdificeId.Count; ++i)
                all.Add(_edifices[i]);
            for (int i = 0; i < WarriorId.Count; ++i)
                all.Add(_warriors[i]);

            Draw_Ed(all, " ALL ");
        }
        public void InputStart_Ed()
        {
            LiteCurrencies start = new() { _roads * 3, _edifices[EdificeId.Camp], _warriors[WarriorId.Militia] };
            Draw_Ed(start, " START ");
        }

        public void InputRoads_Ed(int count) => Draw_Ed(_roads * count, " ROADS ");
        public void InputColonies_Ed(int count) => Draw_Ed(GetColoniesCost() * count, " COLONIES ");
        public void InputPortsOne_Ed(int count) => Draw_Ed(GetPortsOneCost() * count, " PORTS ONE ");
        public void InputPortsTwo_Ed(int count) => Draw_Ed(GetPortsTwoCost() * count, " PORTS TWO ");
        public void InputEdifices_Ed(int coloniesCount, int portsCount, int roadsCount)
        {
            LiteCurrencies edifices = new() { GetColoniesCost() * coloniesCount, GetPortsTwoCost() * portsCount, _roads * roadsCount, };
            Draw_Ed(edifices, " EDIFICES ");
        }

        private void Draw_Ed(ReadOnlyLiteCurrencies currencies, string caption = "===")
        {
            Debug.Log(string.Format("====================={0}=====================", caption));
            System.Text.StringBuilder sb = new();
            for (int i = 0; i < CurrencyId.Count; ++i)
            {
                sb.Append("[<b>"); sb.Append(CurrencyId.Names_Ed[i]); sb.Append("</b>: "); sb.Append(currencies[i].ToString()); sb.Append("] ");
            }
            sb.Append(" |<b>Amount: "); sb.Append(currencies.Amount.ToString()); sb.Append("</b>|");
            Debug.Log(sb.ToString());
        }

        private LiteCurrencies GetColoniesCost() =>  new() { _edifices[EdificeId.Camp], _edifices[EdificeId.Town], _edifices[EdificeId.City], _wall };
        private LiteCurrencies GetPortsOneCost() => new() { _edifices[EdificeId.PortOne], _edifices[EdificeId.LighthouseOne] };
        private LiteCurrencies GetPortsTwoCost() => new() { _edifices[EdificeId.PortTwo], _edifices[EdificeId.LighthouseTwo] };
#endif
    }
}
