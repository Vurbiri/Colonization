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

        [Space]
        [SerializeField] private TextMeshProUGUI _countTMP;
        [SerializeField] private TextMeshProUGUI _profitTMP;

        private Localization _localization;
        private ReactiveCombination<int, int> _reactiveCurrentMax;

        public void Init(IReactiveValue<int> current, IReactiveValue<int> max, ProjectColors colors, CanvasHint hint)
        {
            base.Init(hint);
            _countTMP.color = colors.PanelText;

            _localization = Localization.Instance;
            _reactiveCurrentMax = new(current, max, SetCurrentMax);
        }

        private void SetCurrentMax(int current, int max)
        {
            _countTMP.text = string.Format(CurrentMax.COUNT, current, max);
            _text = _localization.GetFormatText(_getText.id, _getText.key, current, max);
        }

#if UNITY_EDITOR
        public Vector2 Size => ((RectTransform)transform).rect.size;
        public void Init_Editor(ProjectColors settings)
        {
            _countTMP.color = settings.PanelText;
        }

        protected virtual void OnValidate()
        {
            if (_countTMP == null)
                _countTMP = EUtility.GetComponentInChildren<TextMeshProUGUI>(this, "TextTMP");
            if (_profitTMP == null)
                _profitTMP = EUtility.GetComponentInChildren<TextMeshProUGUI>(this, "ProfitTMP");
        }
#endif
    }
}
