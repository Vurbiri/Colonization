using System;

namespace Vurbiri.Reactive.Collections
{
    public class UnsubscriberList<T> : IUnsubscriber
    {
        private IReactiveList<T> _reactive;
        private Action<int, T, Operation> _listener;

        public UnsubscriberList(IReactiveList<T> reactiveList, Action<int, T, Operation> listener)
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
