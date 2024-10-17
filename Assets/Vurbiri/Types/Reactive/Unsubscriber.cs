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

    public class Unsubscriber<TA, TB>
    {
        private IReactive<TA, TB> _reactive;
        private Action<TA, TB> _listener;

        public Unsubscriber(IReactive<TA, TB> reactiveValues, Action<TA, TB> listener)
        {
            _reactive = reactiveValues;
            _listener = listener;
        }

        public Unsubscriber<TA, TB> Unsubscribe()
        {
            _reactive?.Unsubscribe(_listener);
            _reactive = null;
            _listener = null;

            return null;
        }
    }
}
