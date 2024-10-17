using System;

namespace Vurbiri.Reactive
{
    public interface IReactive<T> 
    {
        public Unsubscriber<T> Subscribe(Action<T> action, bool calling = true);
        public void Unsubscribe(Action<T> action);
    }

    public interface IReactive<TA, TB>
    {
        public Unsubscriber<TA, TB> Subscribe(Action<TA, TB> action, bool calling = true);
        public void Unsubscribe(Action<TA, TB> action);
    }
}
