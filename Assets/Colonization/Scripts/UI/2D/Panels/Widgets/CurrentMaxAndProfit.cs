using TMPro;
using UnityEngine;
using Vurbiri.Reactive;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization.UI
{
	sealed public class CurrentMaxAndProfit : ACurrentMax<ReactiveCombination<int, int, int, int>>
    {
        private const string PROFIT = "{0} {1}";

        [Space]
        [SerializeField] private TextMeshProUGUI _profitTMP;

        public void Init(IReactive<int> current, IReactive<int> max, IReactive<int> active, IReactive<int> passive)
        {
            base.Init();
            _reactiveCurrentMax = new(current, max, active, passive, SetCurrentMaxProfit);
        }

        private void SetCurrentMaxProfit(int current, int max, int active, int passive)
        {
            _profitTMP.text = string.Format(PROFIT, NUMBERS_STR[active], NUMBERS_STR[passive]);
            _valueTMP.text = string.Format(COUNT, NUMBERS_STR[current], NUMBERS_STR[max]);
            _hintText = string.Format(_localizedText, NUMBERS_STR[current], NUMBERS_STR[max], NUMBERS_STR[active], NUMBERS_STR[passive], NUMBERS_STR[HEX.GATE]);
        }


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (_profitTMP == null)
                _profitTMP = EUtility.GetComponentInChildren<TextMeshProUGUI>(this, "ProfitTMP");
        }
#endif
    }
}
