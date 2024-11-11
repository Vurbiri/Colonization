using System;

namespace Vurbiri.Reactive.Collections
{
    public class UnsubscriberCollection<T>
    {
        private IReactiveCollection<T> _reactive;
        private Action<T, Operation> _listener;

        public UnsubscriberCollection(IReactiveCollection<T> reactiveList, Action<T, Operation> listener)
        {
            _reactive = reactiveList;
            _listener = listener;
        }

        public UnsubscriberCollection<T> Unsubscribe()
        {
            _reactive?.Unsubscribe(_listener);
            _reactive = null;
            _listener = null;

            return null;
        }
    }
}
