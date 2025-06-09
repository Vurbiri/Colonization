using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class CurrentMaxPopup : ACurrentMax<ReactiveCombination<int, int, int>>
    {
        [Space]
        [SerializeField] private PopupWidgetUI _popup;

        public void Init(IReactive<int, int> current, IReactive<int> max, ProjectColors colors, Direction2 offsetPopup, CanvasHint hint)
        {
            base.Init(colors, hint);

            _popup.Init(colors, offsetPopup);
            _reactiveCurrentMax = new(current, max, SetCurrentDeltaMax);
        }

        private void SetCurrentDeltaMax(int current, int delta, int max)
        {
            _popup.Run(delta);
            SetCurrentMax(current, max);
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
