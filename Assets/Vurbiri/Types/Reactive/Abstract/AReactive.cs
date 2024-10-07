using System;

namespace Vurbiri.Reactive
{
    public abstract class AReactive<T> : IReactive<T>
    {
        protected Action<T> ActionValueChange;

        public Unsubscriber<T> Subscribe(Action<T> action, bool calling = true)
        {
            ActionValueChange -= action;
            ActionValueChange += action;
            if (calling && action != null) 
                Callback(action);

            return new(this, action);
        }

        public void Unsubscribe(Action<T> action) => ActionValueChange -= action;

        protected abstract void Callback(Action<T> action);
    }
}
