//Assets\Colonization\Scripts\UI\_UIGame\Panels\Widget\CurrencyPopup.cs
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class CurrencyPopup : AHintWidget
    {
        [Space]
        [SerializeField] private PopupWidgetUI _popup;

        private Unsubscribers _unsubscribers;

        public void Init(int id, ACurrenciesReactive count, ProjectColors colors, Direction2 offsetPopup, CanvasHint hint)
        {
            base.Init(colors, hint);
            _popup.Init(colors, offsetPopup);

            _unsubscribers += count.Subscribe(id, SetValue);
            _unsubscribers += Localization.Instance.Subscribe(SetHintText);
        }

        private void SetValue(int count)
        {
            _popup.Run(count);
            _valueTMP.text = count.ToString();
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
                _popup = GetComponentInChildren<PopupWidgetUI>(true);
        }
#endif
    }
}
