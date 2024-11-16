using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    public interface IReactiveCollection<T> : IReadOnlyList<T>
    {
        public IUnsubscriber Subscribe(Action<T, Operation> action);
        public void Unsubscribe(Action<T, Operation> action);
    }
}
