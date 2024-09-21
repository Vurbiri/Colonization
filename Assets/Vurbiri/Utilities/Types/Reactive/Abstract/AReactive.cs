using System;

namespace Vurbiri.Reactive
{
    public abstract class AReactive<T> : IReactive<T>
    {
        protected Action<T> EventThisChange;

        public Unsubscriber<T> Subscribe(Action<T> action)
        {
            EventThisChange += action;
            Callback(action);

            return new(this, action);
        }
        public Unsubscriber<T> Subscribe(Action<T> action, bool calling)
        {
            EventThisChange += action;
            if (calling) Callback(action);

            return new(this, action);
        }

        public void Unsubscribe(Action<T> action) => EventThisChange -= action;

        protected abstract void Callback(Action<T> action);
    }
}
