//Assets\Colonization\Scripts\UI\_UIGame\Panels\Widget\CurrentMaxPopup.cs
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class CurrentMaxPopup : CurrentMax
    {
        [Space]
        [SerializeField] private PopupWidgetUI _popup;

        public void Init(IReactiveValue<int> current, IReactiveValue<int> max, ProjectColors settings, Direction2 offsetPopup, CanvasHint hint)
        {
            base.Init(current, max, settings, hint);
            _popup.Init(settings, offsetPopup);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (_popup == null)
                _popup = GetComponentInChildren<PopupWidgetUI>(true);
        }
#endif
    }
}
