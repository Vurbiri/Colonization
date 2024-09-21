using System;

namespace Vurbiri.Reactive
{
    public class Unsubscriber<T>
    {
        private readonly IReactive<T> _reactiveValue;
        private readonly Action<T> _listener;

        public Unsubscriber(IReactive<T> reactiveValue, Action<T> listener)
        {
            _reactiveValue = reactiveValue;
            _listener = listener;
        }

        public void Unsubscribe() => _reactiveValue.Unsubscribe(_listener);
    }
}
