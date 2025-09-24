using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
	sealed public class PlayerCurrencyWidget : ASelectCurrencyCountWidget
    {
        private Action<int, int> a_changeCount;
        private Unsubscription _unsubscriber;

        public void Init(ReadOnlyCurrencies currencies, Action<int, int> action)
        {
            _unsubscriber = currencies.Get(_id).Subscribe(SetMax);
            a_changeCount = action;
            action(_id, _count);
        }

        protected override void SetValue(int value)
        {
            InternalSetValue(value);
            a_changeCount?.Invoke(_id.Value, value);
        }

        private void OnDestroy() => _unsubscriber?.Dispose();
    }
}
