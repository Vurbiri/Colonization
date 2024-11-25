//Assets\Vurbiri\Runtime\Types\Reactive\Unsubscriber.cs
using System;

namespace Vurbiri.Reactive
{
    public class Unsubscriber<TDelegate> : IUnsubscriber where TDelegate : Delegate
    {
        private IReactiveBase<TDelegate> _reactive;
        private TDelegate _listener;

        public Unsubscriber(IReactiveBase<TDelegate> reactiveValue, TDelegate listener)
        {
            _reactive = reactiveValue;
            _listener = listener;
        }

        public void Unsubscribe()
        {
            _reactive?.Unsubscribe(_listener);
            _reactive = null;
            _listener = null;
        }
    }
}
