using System;
using Vurbiri.Reactive;

namespace Vurbiri
{
	public class AReactive<T> : IReactive<T>
    {
        protected T _value;
        protected readonly VAction<T> _eventChanged = new();

        public Subscription Subscribe(Action<T> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, _value);
    }
}
