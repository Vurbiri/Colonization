using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    sealed public class CurrentMaxPopup : ACurrentMax<ReactiveCombination<int, int>>
    {
        [Space]
        [SerializeField] private PopupTextWidgetUI _popup;

        public void Init(Currency blood, IReactive<int> max, Vector3 offsetPopup)
        {
            base.Init();
            _popup.Init(offsetPopup);

            _reactiveCurrentMax = new(blood, max, SetCurrentMax);
            _unsubscribers += blood.SubscribeDelta(_popup.Run);
        }

        protected override void SetLocalizationText(Localization localization)
        {
            _localizedText = localization.GetText(_getText.id, _getText.key);
            if(_reactiveCurrentMax != null)
                _hintText = string.Format(_localizedText, _reactiveCurrentMax.ValueA, _reactiveCurrentMax.ValueB);
        }


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (_popup == null)
                _popup = GetComponentInChildren<PopupTextWidgetUI>(true);
        }
#endif
    }
}
