//Assets\Colonization\Scripts\UI\_UIGame\Currencies\Amount.cs
using TMPro;
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    public class Amount : MonoBehaviour
    {
        private const string AMOUNT = "{0}{1}</color>({2})";

        [SerializeField] private TMP_Text _textTMP;
        [Space]
        [SerializeField] private RectTransform _thisRectTransform;

        private ReactiveCombination<int, int> _reactiveAmountMax;
        private string _colorNormal, _colorOver;

        public Vector2 Size => _thisRectTransform.sizeDelta;

        public void Init(Vector3 position, IReactive<int> amount, IReactive<int> max, TextColorSettings settings)
        {
            _thisRectTransform.localPosition = position;

            _colorNormal = settings.HexColorTextBase;
            _colorOver = settings.HexColorNegative;

            _textTMP.color = settings.ColorTextBase;

            _reactiveAmountMax = new(amount, max);
            _reactiveAmountMax.Subscribe(SetAmountMax);
        }

        private void SetAmountMax(int amount, int max)
        {
            _textTMP.text = string.Format(AMOUNT, amount > max ? _colorOver : _colorNormal, amount, max);
        }

        private void OnDestroy()
        {
            _reactiveAmountMax.Dispose();
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_textTMP == null)
                _textTMP = GetComponent<TMP_Text>();
            if (_thisRectTransform == null)
                _thisRectTransform = GetComponent<RectTransform>();
        }
#endif
    }
}
