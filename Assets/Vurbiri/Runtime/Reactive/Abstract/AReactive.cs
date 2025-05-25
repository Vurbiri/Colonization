using System;
using Vurbiri.Reactive;

namespace Vurbiri
{
	public class AReactive<T> : IReactive<T>
    {
        protected T _value;
        protected readonly Subscription<T> _subscriber = new();

        public Unsubscription Subscribe(Action<T> action, bool instantGetValue = true) => _subscriber.Add(action, instantGetValue, _value);
    }
}
