using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    public interface IReactiveList<T> : IReadOnlyList<T>
    {
        public UnsubscriberList<T> Subscribe(Action<int, T, Operation> action, bool calling = true);
        public void Unsubscribe(Action<int, T, Operation> action);
    }
}
