using TMPro;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    sealed public class CurrentMaxAndRateProfit : ACurrentMax<ReactiveCombination<int, int, int>>
    {
        private const string RATE = "x{0}";

        [Space]
        [SerializeField] private TextMeshProUGUI _profitTMP;

        public void Init(IReactive<int> current, IReactive<int> max, IReactive<int> rateProfit)
        {
            base.Init();
            _reactiveCurrentMax = new(current, max, rateProfit, SetCurrentMaxRateProfit);
        }

        private void SetCurrentMaxRateProfit(int current, int max, int rate)
        {
            _profitTMP.text = string.Format(RATE, CONST.NUMBERS_STR[++rate]);
            _valueTMP.text = string.Format(COUNT, CONST.NUMBERS_STR[current], CONST.NUMBERS_STR[max]);
            _hintText = string.Format(_localizedText, CONST.NUMBERS_STR[current], CONST.NUMBERS_STR[max], CONST.NUMBERS_STR[rate]);
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
