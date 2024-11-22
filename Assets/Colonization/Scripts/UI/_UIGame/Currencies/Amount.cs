//Assets\Colonization\Scripts\UI\_UIGame\Currencies\Amount.cs
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

        private ReactiveCombination<int, int> _reactiveAmountMax;

        public Vector2 Size => _thisRectTransform.sizeDelta;

        public void Init(Vector3 position, IReactive<int> amount, IReactive<int> max)
        {
            _thisRectTransform.localPosition = position;

            _reactiveAmountMax = new(amount, max);
            _reactiveAmountMax.Subscribe(SetAmountMax);
        }

        private void SetAmountMax(int amount, int max)
        {
            _amountTMP.text = amount.ToString();
            _maxTMP.text = max.ToString();

            _amountTMP.color = amount > max ? _colorOver : _colorNormal;
        }

        private void OnDestroy()
        {
            _reactiveAmountMax.Dispose();
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
