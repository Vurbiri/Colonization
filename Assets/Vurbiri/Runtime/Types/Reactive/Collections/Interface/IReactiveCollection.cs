using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    public interface IReactiveCollection<T> : IReadOnlyList<T>
    {
        public UnsubscriberCollection<T> Subscribe(Action<T, Operation> action, bool calling = true);
        public void Unsubscribe(Action<T, Operation> action);
    }
}
