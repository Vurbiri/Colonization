//Assets\Colonization\Scripts\UI\Utilities\PopupWidget\PopupWidgetUI.cs
using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class PopupWidgetUI : APopupWidget<TMP_Text>
    {
        protected int _prevValue = int.MinValue;

        public void Init(Direction2 direction)
        {
            base.Init(direction);
        }

        public void Run(int value)
        {
            int delta = value - _prevValue;

            if (_prevValue < 0 || delta == 0)
            {
                _prevValue = value;
                return;
            }

            _self.SetActive(true);
            _queue.Add(Run_Coroutine(string.Format(delta > 0 ? _stringPlus : _stringMinus, delta)));

            _prevValue = value;
        }
    }
}
