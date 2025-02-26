//Assets\Colonization\Scripts\UI\_UIGame\Currencies\Currency.cs
using TMPro;
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;

    public class Currency : MonoBehaviour
    {
        private const string COUNT = "{0}";

        [SerializeField] private TMP_Text _textTMP;
        [SerializeField] private PopupWidgetUI _popup;
        [Space]
        [SerializeField] private RectTransform _thisRectTransform;

        private string _currency;
        private Unsubscriber _unsubscriber;
        
        public Vector2 Size => _thisRectTransform.sizeDelta;

        public void Init(int id, Vector3 position, ACurrenciesReactive count, TextColorSettings settings, Direction2 offsetPopup)
        {
            _popup.Init(settings, offsetPopup);
            _thisRectTransform.localPosition = position;

            _currency = string.Format(TAG_SPRITE, id).Concat(COUNT);
            _textTMP.color = settings.ColorTextBase;

            _unsubscriber = count.Subscribe(id, SetValue);
        }

        private void SetValue(int count)
        {
            _popup.Run(count);
            _textTMP.text = string.Format(_currency, count);
        }

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_textTMP == null)
                _textTMP = GetComponent<TMP_Text>();
            if (_thisRectTransform == null)
                _thisRectTransform = GetComponent<RectTransform>();
            if(_popup == null)
                _popup = GetComponentInChildren<PopupWidgetUI>();
        }
#endif
    }
}
