using TMPro;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class Amount : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountTMP;
        [SerializeField] private TMP_Text _maxTMP;
        [Space]
        [SerializeField] private Color _colorNormal = Color.white;
        [SerializeField] private Color _colorOver = Color.red;
        [Space]
        [SerializeField] private RectTransform _thisRectTransform;

        private AReadOnlyCurrenciesReactive _reactiveAmount;
        private IReadOnlyReactiveValue<int> _reactiveMax;

        public Vector2 Size => _thisRectTransform.sizeDelta;

        public void Init(Vector3 position, AReadOnlyCurrenciesReactive amount, IReadOnlyReactiveValue<int> max)
        {
            _thisRectTransform.localPosition = position;

            _reactiveAmount = amount;
            _reactiveMax = max;

            amount.Subscribe(SetAmount, false);
            max.Subscribe(SetAmount);
        }

        private void SetAmount(int value)
        {
            _amountTMP.text = _reactiveAmount.Amount.ToString();
            _maxTMP.text = _reactiveMax.Value.ToString();

            _amountTMP.color = _reactiveAmount.Amount > _reactiveMax.Value ? _colorOver : _colorNormal;
        }

        private void OnDestroy()
        {
            _reactiveAmount?.Unsubscribe(SetAmount);
            _reactiveMax?.Unsubscribe(SetAmount);
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_thisRectTransform == null)
                _thisRectTransform = GetComponent<RectTransform>();
        }
#endif
    }
}
