//Assets\Colonization\Scripts\UI\_UIGame\Panels\Widget\CurrentMaxPopup.cs
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    public class CurrentMaxPopup : CurrentMax
    {
        [SerializeField] private PopupWidgetUI _popup;

        public void Init(IReactiveValue<int> current, IReactiveValue<int> max, ProjectColors settings, Direction2 offsetPopup)
        {
            base.Init(current, max, settings);
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
