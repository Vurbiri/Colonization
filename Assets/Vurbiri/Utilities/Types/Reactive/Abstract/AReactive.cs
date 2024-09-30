using System;

namespace Vurbiri.Reactive
{
    public abstract class AReactive<T> : IReactive<T>
    {
        protected Action<T> ActionThisChange;

        public Unsubscriber<T> Subscribe(Action<T> action, bool calling = true)
        {
            ActionThisChange -= action;
            ActionThisChange += action;
            if (calling && action != null) 
                Callback(action);

            return new(this, action);
        }

        public void Unsubscribe(Action<T> action) => ActionThisChange -= action;

        protected abstract void Callback(Action<T> action);
    }
}
