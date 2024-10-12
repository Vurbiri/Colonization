using System;

namespace Vurbiri.Reactive
{
    public class Unsubscriber<T>
    {
        private IReactive<T> _reactive;
        private Action<T> _listener;

        public Unsubscriber(IReactive<T> reactiveValue, Action<T> listener)
        {
            _reactive = reactiveValue;
            _listener = listener;
        }

        public Unsubscriber<T> Unsubscribe()
        {
            _reactive?.Unsubscribe(_listener);
            _reactive = null;
            _listener = null;

            return null;
        }
    }
}
