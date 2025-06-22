using UnityEngine;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class CurrencyPopup : AHintWidget
    {
        [Space]
        [SerializeField] private PopupTextWidgetUI _popup;

        public void Init(int id, ACurrenciesReactive currencies, ProjectColors colors, Direction2 offsetPopup, CanvasHint hint)
        {
            base.Init(colors, hint);
            _popup.Init(colors, offsetPopup);

            var currency = currencies.Get(id);
            _unsubscribers += currency.Subscribe(SetValue);
            _unsubscribers += currency.SubscribeDelta(_popup.Run);
        }

        private void SetValue(int value) => _valueTMP.text = value.ToString();

        protected override void SetLocalizationText(Localization localization)
        {
            _text = localization.GetText(_getText.id, _getText.key);
        }

#if UNITY_EDITOR

        public void Init_Editor(int id, Vector3 position, ProjectColors settings)
        {
            Init_Editor(settings);
            transform.localPosition = position;

            UnityEditor.SerializedObject self = new(this);
            self.FindProperty("_getText").FindPropertyRelative("key").stringValue = CurrencyId.Names[id];
            self.ApplyModifiedProperties();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (_popup == null)
                _popup = GetComponentInChildren<PopupTextWidgetUI>(true);
        }
#endif
    }
}
