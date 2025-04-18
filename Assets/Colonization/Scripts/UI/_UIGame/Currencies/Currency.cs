//Assets\Colonization\Scripts\UI\_UIGame\Currencies\Currency.cs
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vurbiri.Collections;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class Currency : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _countTMP;
        [SerializeField] private PopupWidgetUI _popup;
        [Space]
        [SerializeField] private RectTransform _thisRectTransform;

        private Unsubscriber _unsubscriber;
        
        public Vector2 Size => _thisRectTransform.sizeDelta;

        public void Init(int id, IdArray<CurrencyId, CurrencyIcon> icons, Vector3 position, ACurrenciesReactive count, TextColorSettings settings, Direction2 offsetPopup)
        {
            _popup.Init(settings, offsetPopup);
            _thisRectTransform.localPosition = position;

            icons[id].ToImage(_icon);
            _countTMP.color = settings.ColorTextBase;

            _unsubscriber = count.Subscribe(id, SetValue);
        }

        private void SetValue(int count)
        {
            _popup.Run(count);
            _countTMP.text = Mathf.Min(count, 99).ToString();
        }

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPointerEnter ");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("OnPointerExit ");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_icon == null)
                _icon = GetComponentInChildren<Image>();
            if (_countTMP == null)
                _countTMP = GetComponent<TMP_Text>();
            if (_thisRectTransform == null)
                _thisRectTransform = GetComponent<RectTransform>();
            if(_popup == null)
                _popup = GetComponentInChildren<PopupWidgetUI>();
        }


#endif
    }
}
