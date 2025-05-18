//Assets\Colonization\Scripts\UI\_UIGame\Panels\Widget\CurrencyPopup.cs
using TMPro;
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class CurrencyPopup : AHintWidget
    {
        [SerializeField] private TextMeshProUGUI _countTMP;
        [SerializeField] private PopupWidgetUI _popup;

        private Unsubscriber _unsubscriber;

        public void Init(int id, ACurrenciesReactive count, ProjectColors settings, Direction2 offsetPopup, CanvasHint hint)
        {
            base.Init(hint);
            _popup.Init(settings, offsetPopup);
            
            _countTMP.color = settings.PanelText;
            _unsubscriber = count.Subscribe(id, SetValue);
        }

        private void SetValue(int count)
        {
            _popup.Run(count);
            _countTMP.text = count.ToString();
        }

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }

#if UNITY_EDITOR

        public Vector2 Size => ((RectTransform)transform).sizeDelta;
        public void Init_Editor(Vector3 position, ProjectColors settings)
        {
            ((RectTransform)transform).localPosition = position;
            _countTMP.color = settings.PanelText;
        }

        private void OnValidate()
        {
            if (_countTMP == null)
                _countTMP = EUtility.GetComponentInChildren<TextMeshProUGUI>(this, "TextTMP");
            if (_popup == null)
                _popup = GetComponentInChildren<PopupWidgetUI>(true);

        }
#endif
    }
}
