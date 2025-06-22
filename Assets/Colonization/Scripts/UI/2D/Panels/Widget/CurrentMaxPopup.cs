using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;
using static Vurbiri.Colonization.ACurrenciesReactive;

namespace Vurbiri.Colonization.UI
{
    sealed public class CurrentMaxPopup : ACurrentMax<ReactiveCombination<int, int>>
    {
        [Space]
        [SerializeField] private PopupTextWidgetUI _popup;

        public void Init(ICurrency blood, IReactive<int> max, ProjectColors colors, Direction2 offsetPopup, CanvasHint hint)
        {
            base.Init(colors, hint);
            _popup.Init(colors, offsetPopup);

            _reactiveCurrentMax = new(blood, max, SetCurrentMax);
            _unsubscribers += blood.SubscribeDelta(_popup.Run);
        }

        protected override void SetLocalizationText(Localization localization)
        {
            _textHint = localization.GetText(_getText.id, _getText.key);
            if(_reactiveCurrentMax != null)
                _text = string.Format(_textHint, _reactiveCurrentMax.ValueA, _reactiveCurrentMax.ValueB);
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
