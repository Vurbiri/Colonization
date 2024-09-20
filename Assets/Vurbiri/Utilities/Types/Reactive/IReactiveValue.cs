using System;

namespace Vurbiri
{
    public interface IReactiveValue<T> where T : IReactiveValue<T>
    {
        protected event Action<T> EventValueChange;

        public virtual void Subscribe(Action<T> action)
        {
            EventValueChange += action;
            action((T)this);
        }

        public virtual void UnSubscribe(Action<T> action) => EventValueChange -= action;
    }
}
