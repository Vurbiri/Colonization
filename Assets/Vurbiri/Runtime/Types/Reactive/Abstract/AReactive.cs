using System;

namespace Vurbiri.Reactive
{
    public abstract class AReactive<T> : IReadOnlyReactiveValue<T>
    {
        protected Action<T> actionValueChange;

        public abstract T Value { get; protected set; }

        public Unsubscriber<T> Subscribe(Action<T> action, bool calling = true)
        {
            actionValueChange -= action ?? throw new ArgumentNullException("action");

            actionValueChange += action;
            if (calling)
                action(Value);

            return new(this, action);
        }

        public void Unsubscribe(Action<T> action) => actionValueChange -= action ?? throw new ArgumentNullException("action");
    }
}
