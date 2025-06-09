using TMPro;
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class CurrentMaxAndRateProfit : ACurrentMax<ReactiveCombination<int, int, int>>
    {
        private const string RATE = "x{0}";

        [Space]
        [SerializeField] private TextMeshProUGUI _profitTMP;

        public void Init(IReactive<int> current, IReactive<int> max, IReactive<int> rateProfit, ProjectColors colors, CanvasHint hint)
        {
            base.Init(colors, hint);

            _reactiveCurrentMax = new(current, max, rateProfit, SetCurrentMaxRateProfit);
        }

        private void SetCurrentMaxRateProfit(int current, int max, int rate)
        {
            _profitTMP.text = string.Format(RATE, ++rate);
            _valueTMP.text = string.Format(COUNT, current, max);
            _text = string.Format(_textHint, current, max, rate);
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
