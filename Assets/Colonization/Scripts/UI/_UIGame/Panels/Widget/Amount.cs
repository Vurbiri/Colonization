//Assets\Colonization\Scripts\UI\_UIGame\Panels\Widget\Amount.cs
using TMPro;
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class Amount : AHintWidget
    {
        private const string AMOUNT = "{0,2}{1}</color><space=0.05em>|<space=0.05em>{2,-2}";

        [SerializeField] private TextMeshProUGUI _textTMP;

        private ReactiveCombination<int, int> _reactiveAmountMax;
        private string _colorNormal, _colorOver;

        public void Init(IReactiveValue<int> amount, IReactiveValue<int> max, ProjectColors settings, CanvasHint hint)
        {
            base.Init(hint);

            _colorNormal = settings.PanelTextTag;
            _colorOver = settings.TextNegativeTag;

            _textTMP.color = settings.PanelText;

            _reactiveAmountMax = new(amount, max, SetAmountMax);
        }

        private void SetAmountMax(int amount, int max)
        {
            _textTMP.text = string.Format(AMOUNT, amount > max ? _colorOver : _colorNormal, Mathf.Min(amount, 99), max);
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
            _textTMP.text = string.Format(AMOUNT, "<#ff0000ff>", Mathf.Min(33, 99), 25);
        }
        private void OnValidate()
        {
            if (_textTMP == null)
                _textTMP = EUtility.GetComponentInChildren<TextMeshProUGUI>(this, "TextTMP");
        }
#endif
    }
}
