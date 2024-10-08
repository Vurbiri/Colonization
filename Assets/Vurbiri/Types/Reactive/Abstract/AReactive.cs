using System;

namespace Vurbiri.Reactive
{
    public abstract class AReactive<T> : IReadOnlyReactiveValue<T>
    {
        protected Action<T> ActionValueChange;

        public abstract T Value { get; protected set; }

        public Unsubscriber<T> Subscribe(Action<T> action, bool calling = true)
        {
            ActionValueChange -= action;
            ActionValueChange += action;
            if (calling && action != null)
                action(Value);

            return new(this, action);
        }

        public void Unsubscribe(Action<T> action) => ActionValueChange -= action;
    }
}
