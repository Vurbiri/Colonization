using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
	sealed public class PlayerCurrencyWidget : ASelectCurrencyCountWidget
    {
        private readonly Subscription<int, int> _changeCount = new();
        private Unsubscription _unsubscriber;

        public void Init(ACurrenciesReactive currencies, Action<int, int> action)
        {
            _unsubscriber = currencies.Get(_id).Subscribe(SetMax);
            _changeCount.Add(action);
        }

        protected override void SetValue(int value)
        {
            base.SetValue(value);

            _changeCount.Invoke(_id.Value, value);
        }

        public void SetMax(int value)
        {
            _max = value;
            CrossFadeColor();

            if (_count > _max)
            {
                _count = _max;
                SetValue(_count);
            }
        }

        private void OnDestroy() => _unsubscriber?.Unsubscribe();

#if UNITY_EDITOR

#endif
    }
}
