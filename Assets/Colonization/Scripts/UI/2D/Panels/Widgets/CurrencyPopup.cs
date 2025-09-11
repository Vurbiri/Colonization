using UnityEngine;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class CurrencyPopup : AHintWidget
    {
        [Space]
        [SerializeField] private PopupTextWidgetUI _popup;

        public void Init(int id, ReadOnlyCurrencies currencies, Direction2 offsetPopup)
        {
            base.Init();
            _popup.Init(offsetPopup);

            var currency = currencies.Get(id);
            _unsubscribers += currency.Subscribe(SetValue);
            _unsubscribers += currency.SubscribeDelta(_popup.Run);
        }

        private void SetValue(int value) => _valueTMP.text = value.ToString();

        protected override void SetLocalizationText(Localization localization)
        {
            _hintText = localization.GetText(_getText.id, _getText.key);
        }

#if UNITY_EDITOR

        public void Init_Editor(int id, Vector3 position, ProjectColors colors)
        {
            Init_Editor(colors);
            transform.localPosition = position;

            string name = CurrencyId.Names_Ed[id];
            UnityEditor.SerializedObject so = new(this);
            so.FindProperty("_getText").FindPropertyRelative("key").stringValue = name;
            so.ApplyModifiedProperties();

            name = $"{id}_{name}";
            so = new(EUtility.GetComponentInChildren<UnityEngine.UI.Image>(this, "Icon"));
            so.FindProperty("m_Sprite").objectReferenceValue = EUtility.FindMultipleSprite("SPA_C".Concat(name));
            so.ApplyModifiedProperties();

            gameObject.name = name;
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
