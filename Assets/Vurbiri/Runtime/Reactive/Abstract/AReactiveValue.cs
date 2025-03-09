//Assets\Vurbiri\Runtime\Reactive\Abstract\AReactiveValue.cs
using System;

namespace Vurbiri.Reactive
{
    public abstract class AReactiveValue<T> : IReactiveValue<T>
    {
        protected Subscriber<T> _subscriber = new();

        public abstract T Value { get; protected set; }

        public Unsubscriber Subscribe(Action<T> action, bool calling = true)
        {
            if (calling)
                action(Value);

            return _subscriber.Add(action);
        }
    }
}
