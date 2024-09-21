using System;

namespace Vurbiri.Reactive
{
    public interface IReactive<T>
    {
        public Unsubscriber<T> Subscribe(Action<T> action);
        public Unsubscriber<T> Subscribe(Action<T> action, bool calling);

        public void Unsubscribe(Action<T> action);
    }
}
