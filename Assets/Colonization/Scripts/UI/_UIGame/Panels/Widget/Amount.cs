//Assets\Colonization\Scripts\UI\_UIGame\Panels\Widget\Amount.cs
using TMPro;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class Amount : AHintWidget
    {
        private const string AMOUNT = "{0}{1,2}</color><space=0.05em>|<space=0.05em>{2,-2}";

        [SerializeField] private string _keyOver;
        [Space]
        [SerializeField] private TextMeshProUGUI _textTMP;

        private Localization _localization;
        private ReactiveCombination<int, int> _reactiveAmountMax;
        private string _colorNormal, _colorOver;

        public void Init(IReactiveValue<int> amount, IReactiveValue<int> max, ProjectColors settings, CanvasHint hint)
        {
            base.Init(hint);

            _colorNormal = settings.TextPositiveTag;
            _colorOver = settings.TextNegativeTag;

            _textTMP.color = settings.PanelText;

            _localization = Localization.Instance;
            _reactiveAmountMax = new(amount, max, SetAmountMax);
        }

        private void SetAmountMax(int amount, int max)
        {
            string color, over;
            if(amount > max)
            {
                color = _colorOver;
                over = _localization.GetText(_getText.id, _keyOver);
            }
            else 
            {
                color = _colorNormal;
                over = string.Empty;
            }

            _textTMP.text = string.Format(AMOUNT, color, Mathf.Min(amount, 99), max);
            _text = _localization.GetFormatText(_getText.id, _getText.key, color, amount, max).Concat(over);
        }

        private void OnDestroy()
        {
            _reactiveAmountMax.Dispose();
        }

#if UNITY_EDITOR
        public Vector2 Size => ((RectTransform)transform).sizeDelta;
        public void Init_Editor(Vector3 position, ProjectColors settings)
        {
            ((RectTransform)transform).localPosition = position;

            _textTMP.color = settings.PanelText;
        }
        private void OnValidate()
        {
            if (_textTMP == null)
                _textTMP = EUtility.GetComponentInChildren<TextMeshProUGUI>(this, "TextTMP");
        }
#endif
    }
}
