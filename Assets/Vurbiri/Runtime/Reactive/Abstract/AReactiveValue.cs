//Assets\Vurbiri\Runtime\Reactive\Abstract\AReactiveValue.cs
using System;

namespace Vurbiri.Reactive
{
    public abstract class AReactiveValue<T> : IReactiveValue<T>
    {
        protected readonly Subscription<T> _subscriber = new();

        public abstract T Value { get; protected set; }

        public Unsubscription Subscribe(Action<T> action, bool instantGetValue = true) => _subscriber.Add(action, instantGetValue, Value);
    }
}
