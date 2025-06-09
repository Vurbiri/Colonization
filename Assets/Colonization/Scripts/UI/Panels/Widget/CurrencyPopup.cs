using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class CurrencyPopup : AHintWidget
    {
        [Space]
        [SerializeField] private PopupWidgetUI _popup;

        public void Init(int id, ACurrenciesReactive count, ProjectColors colors, Direction2 offsetPopup, CanvasHint hint)
        {
            base.Init(colors, hint);
            _popup.Init(colors, offsetPopup);

            _unsubscribers += count.Subscribe(id, SetValue);
        }

        private void SetValue(int current, int delta)
        {
            _popup.Run(delta);
            _valueTMP.text = current.ToString();
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
