//Assets\Vurbiri\Runtime\Types\Reactive\Abstract\AReactive.cs
using System;

namespace Vurbiri.Reactive
{
    public abstract class AReactive<T> : IReadOnlyReactive<T>
    {
        protected Subscriber<T> _subscriber = new();

        public abstract T Value { get; protected set; }

        public IUnsubscriber Subscribe(Action<T> action, bool calling = true)
        {
            if (calling)
                action(Value);

            return _subscriber.Add(action);
        }

        public virtual void Dispose()
        {
            _subscriber.Dispose();
        }
    }
}
