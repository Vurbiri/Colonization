using System;

namespace Vurbiri.Reactive.Collections
{
    public class UnsubscriberCollection<T> : IUnsubscriber
    {
        private IReactiveCollection<T> _reactive;
        private Action<T, Operation> _listener;

        public UnsubscriberCollection(IReactiveCollection<T> reactiveList, Action<T, Operation> listener)
        {
            _reactive = reactiveList;
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
