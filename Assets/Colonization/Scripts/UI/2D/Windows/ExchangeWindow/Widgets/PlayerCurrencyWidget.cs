using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
	sealed public class PlayerCurrencyWidget : ASelectCurrencyCountWidget
    {
        private Unsubscription _unsubscriber;

        public void Init(ACurrenciesReactive currencies, Action<int, int> action)
        {
            _unsubscriber = currencies.Get(_id).Subscribe(SetMax);
            _changeCount.Add(action);
        }

        private void SetMax(int value)
        {
            _max = value;

            if(_count > _max)
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
