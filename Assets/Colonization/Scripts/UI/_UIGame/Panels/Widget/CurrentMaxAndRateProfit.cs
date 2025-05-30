using TMPro;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public class CurrentMaxAndRateProfit : AHintWidget
    {
        private const string RATE = "x{0}";

        [SerializeField, Key(Files.Gameplay)] private string _keyRateProfit;
        [Space]
        [SerializeField] private TextMeshProUGUI _profitTMP;

        private Localization _localization;
        private ReactiveCombination<int, int> _reactiveCurrentMax;
        private Unsubscription _unsubscription;

        private string _textCurrentMax, _textRateProfit;

        public void Init(IReactiveValue<int> current, IReactiveValue<int> max, IReactiveValue<int> rateProfit, ProjectColors colors, CanvasHint hint)
        {
            base.Init(colors, hint);

            _localization = Localization.Instance;
            _reactiveCurrentMax = new(current, max, SetCurrentMax);
            _unsubscription = rateProfit.Subscribe(SetRateProfit);
        }

        private void SetCurrentMax(int current, int max)
        {
            _valueTMP.text = string.Format(CurrentMax.COUNT, current, max);
            _textCurrentMax = _localization.GetFormatText(_getText.id, _getText.key, current, max);
            _text = string.Concat(_textCurrentMax, _textRateProfit);
        }
        private void SetRateProfit(int rate)
        {
            _profitTMP.text = string.Format(RATE, ++rate);
            _textRateProfit = _localization.GetFormatText(_getText.id, _keyRateProfit, rate);
            _text = string.Concat(_textCurrentMax, _textRateProfit);
        }

        private void OnDestroy()
        {
            _reactiveCurrentMax.Dispose();
            _unsubscription.Unsubscribe();
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
