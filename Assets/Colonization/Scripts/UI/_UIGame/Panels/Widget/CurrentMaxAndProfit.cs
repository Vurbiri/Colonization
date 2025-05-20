//Assets\Colonization\Scripts\UI\_UIGame\Panels\Widget\CurrentMaxAndProfit.cs
using TMPro;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public class CurrentMaxAndProfit : AHintWidget
    {
        private const string PROFIT = "{0} {1}";

        [SerializeField] private string _keyProfit;
        [Space]
        [SerializeField] private TextMeshProUGUI _profitTMP;

        private Localization _localization;
        private ReactiveCombination<int, int> _reactiveCurrentMax, _reactiveProfit;

        private string _textCurrentMax, _textProfit;

        public void Init(IReactiveValue<int> current, IReactiveValue<int> max, IReactiveValue<int> activeProfit, IReactiveValue<int> passiveProfit, ProjectColors colors, CanvasHint hint)
        {
            base.Init(colors, hint);

            _localization = Localization.Instance;
            _reactiveCurrentMax = new(current, max, SetCurrentMax);
            _reactiveProfit = new(activeProfit, passiveProfit, SetProfit);
        }

        private void SetCurrentMax(int current, int max)
        {
            _valueTMP.text = string.Format(CurrentMax.COUNT, current, max);
            _textCurrentMax = _localization.GetFormatText(_getText.id, _getText.key, current, max);
            _text = string.Concat(_textCurrentMax, _textProfit);
        }
        private void SetProfit(int active, int passive)
        {
            _profitTMP.text = string.Format(PROFIT, active, passive);
            _textProfit = _localization.GetFormatText(_getText.id, _keyProfit, active, passive);
            _text = string.Concat(_textCurrentMax, _textProfit);
        }

        private void OnDestroy()
        {
            _reactiveCurrentMax.Dispose();
            _reactiveProfit.Dispose();
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
