using TMPro;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    public class Blood : MonoBehaviour
    {
        [SerializeField] private TMP_Text _countTMP;
        [SerializeField] private TMP_Text _maxTMP;
        [SerializeField] private CurrencyWidget _popup;
        [Space]
        [SerializeField] private RectTransform _thisRectTransform;

        private Unsubscribers _unsubscribers = new(2);

        public Vector2 Size => _thisRectTransform.sizeDelta;

        public void Init(Vector3 position, ACurrenciesReactive count, IReactive<int> max, Vector3 offsetPopup)
        {
            _popup.Init(offsetPopup);
            _thisRectTransform.localPosition = position;

            _unsubscribers += count.Subscribe(CurrencyId.Blood, SetValue);
            _unsubscribers += max.Subscribe((v) => _maxTMP.text = v.ToString());
        }

        private void SetValue(int count)
        {
            _popup.Run(count);
            _countTMP.text = count.ToString();
        }

        private void OnDestroy()
        {
            _unsubscribers.Unsubscribe();
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
