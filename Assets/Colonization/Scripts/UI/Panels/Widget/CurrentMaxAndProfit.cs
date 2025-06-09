using TMPro;
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public class CurrentMaxAndProfit : ACurrentMax<ReactiveCombination<int, int, int, int>>
    {
        private const string PROFIT = "{0} {1}";

        [Space]
        [SerializeField] private TextMeshProUGUI _profitTMP;

        public void Init(IReactiveValue<int> current, IReactiveValue<int> max, IReactiveValue<int> activeProfit, IReactiveValue<int> passiveProfit, ProjectColors colors, CanvasHint hint)
        {
            base.Init(colors, hint);

            _reactiveCurrentMax = new(current, max, activeProfit, passiveProfit, SetCurrentMaxProfit);
        }

        private void SetCurrentMaxProfit(int current, int max, int active, int passive)
        {
            _profitTMP.text = string.Format(PROFIT, active, passive);
            _valueTMP.text = string.Format(COUNT, current, max);
            _text = string.Format(_textHint, current, max, active, passive);
        }

        //private void SetCurrentMax(int current, int max)
        //{
        //    _valueTMP.text = string.Format(CurrentMax.COUNT, current, max);
        //    _textCurrentMax = _localization.GetFormatText(_getText.id, _getText.key, current, max);
        //    _text = string.Concat(_textCurrentMax, _textProfit);
        //}
        //private void SetProfit(int active, int passive)
        //{
        //    _profitTMP.text = string.Format(PROFIT, active, passive);
        //    _textProfit = _localization.GetFormatText(_getText.id, _keyProfit, active, passive);
        //    _text = string.Concat(_textCurrentMax, _textProfit);
        //}


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
