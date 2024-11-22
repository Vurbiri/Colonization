//Assets\Vurbiri\Runtime\Types\Reactive\Abstract\AReactive.cs
using System;

namespace Vurbiri.Reactive
{
    public abstract class AReactive<T> : IReadOnlyReactive<T>
    {
        protected Action<T> actionValueChange;

        public abstract T Value { get; protected set; }

        public IUnsubscriber Subscribe(Action<T> action, bool calling = true)
        {
            actionValueChange -= action ?? throw new ArgumentNullException("action");

            actionValueChange += action;
            if (calling)
                action(Value);

            return new Unsubscriber<Action<T>>(this, action);
        }

        public void Unsubscribe(Action<T> action) => actionValueChange -= action ?? throw new ArgumentNullException("action");
    }
}
