//Assets\Colonization\Scripts\UI\_UIGame\Panels\Widget\CurrencyPopup.cs
using TMPro;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class CurrencyPopup : AHintWidget
    {
        [Space]
        [SerializeField] private TextMeshProUGUI _countTMP;
        [SerializeField] private PopupWidgetUI _popup;

        private Unsubscribers _unsubscribers;

        public void Init(int id, ACurrenciesReactive count, ProjectColors settings, Direction2 offsetPopup, CanvasHint hint)
        {
            base.Init(hint);
            _popup.Init(settings, offsetPopup);
            
            _countTMP.color = settings.PanelText;

            _unsubscribers += count.Subscribe(id, SetValue);
            _unsubscribers += Localization.Instance.Subscribe(SetHintText);
        }

        private void SetValue(int count)
        {
            _popup.Run(count);
            _countTMP.text = count.ToString();
        }

        private void SetHintText(Localization localization)
        {
            _text = localization.GetText(_getText.id, _getText.key);
        }

        private void OnDestroy()
        {
            _unsubscribers?.Unsubscribe();
        }

#if UNITY_EDITOR

        public Vector2 Size => ((RectTransform)transform).sizeDelta;
        public void Init_Editor(int id, Vector3 position, ProjectColors settings)
        {
            ((RectTransform)transform).localPosition = position;
            _countTMP.color = settings.PanelText;

            UnityEditor.SerializedObject self = new(this);
            self.FindProperty("_getText").FindPropertyRelative("key").stringValue = CurrencyId.Names[id];
            self.ApplyModifiedProperties();
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
