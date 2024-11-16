using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    public interface IReactiveList<T> : IReadOnlyList<T>
    {
        public IUnsubscriber Subscribe(Action<int, T, Operation> action);
        public void Unsubscribe(Action<int, T, Operation> action);
    }
}
