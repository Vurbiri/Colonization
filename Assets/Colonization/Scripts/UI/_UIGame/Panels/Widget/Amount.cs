//Assets\Colonization\Scripts\UI\_UIGame\Panels\Widget\Amount.cs
using TMPro;
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    public class Amount : MonoBehaviour
    {
        private const string AMOUNT = "{0}{1}</color><space=0.14em>({2})";

        [SerializeField] private TMP_Text _textTMP;

        private ReactiveCombination<int, int> _reactiveAmountMax;
        private string _colorNormal, _colorOver;

        public void Init(IReactiveValue<int> amount, IReactiveValue<int> max, TextColorSettings settings)
        {
            _colorNormal = settings.HexColorTextBase;
            _colorOver = settings.HexColorNegative;

            _textTMP.color = settings.ColorTextBase;

            _reactiveAmountMax = new(amount, max);
            _reactiveAmountMax.Subscribe(SetAmountMax);
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
        public void Init_Editor(Vector3 position, TextColorSettings settings)
        {
            ((RectTransform)transform).localPosition = position;

            _textTMP.color = settings.ColorTextBase;
        }
        private void OnValidate()
        {
            if (_textTMP == null)
                _textTMP = GetComponent<TMP_Text>();
        }
#endif
    }
}
