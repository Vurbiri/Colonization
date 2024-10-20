using System;

namespace Vurbiri.Reactive
{
    public interface IReactiveSubValues<T, U> : IReactive<T> where U : AIdType<U>
    {
        public int this[int index] { get; }
        public int this[Id<U> id] { get; }

        public Unsubscriber<int> Subscribe(int index, Action<int> action, bool calling = true);
        public Unsubscriber<int> Subscribe(Id<U> id, Action<int> action, bool calling = true);
        public void Unsubscribe(int index, Action<int> action);
        public void Unsubscribe(Id<U> id, Action<int> action);
    }
}
