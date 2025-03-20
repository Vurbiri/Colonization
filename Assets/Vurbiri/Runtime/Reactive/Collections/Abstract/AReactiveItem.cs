//Assets\Vurbiri\Runtime\Reactive\Collections\Abstract\AReactiveItem.cs
using System;

namespace Vurbiri.Reactive.Collections
{
    public abstract class AReactiveItem<T> : IReactiveItem<T> where T : AReactiveItem<T>
	{
        protected readonly Subscriber<T, TypeEvent> _subscriber = new();
        protected int _index = -1;

        public int Index { get => _index; set { _index = value; _subscriber.Invoke((T)this, TypeEvent.Reindex); } }

        public void Adding(Action<T, TypeEvent> action, int index)
        {
            _index = index;
            action((T)this, TypeEvent.Add);
            _subscriber.Add(action);
        }

        public Unsubscriber Subscribe(Action<T, TypeEvent> action, bool calling = true)
        {
            if (calling) action((T)this, TypeEvent.Subscribe);
            return _subscriber.Add(action);
        }

        public virtual void Removing()
        {
            _subscriber.Invoke((T)this, TypeEvent.Remove);
            _subscriber.Dispose();
            _index = -1;
        }

        public abstract bool Equals(T other);
        public abstract void Dispose();
    }
}
