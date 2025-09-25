using System;
using UnityEngine;

namespace Vurbiri.Reactive.Collections
{
    public abstract class AReactiveItemMono<T> : MonoBehaviour, IReactiveItem<T> where T : AReactiveItemMono<T>
    {
        protected readonly VAction<T, TypeEvent> _eventChanged = new();
        protected int _index = -1;

        public int Index => _index;

        public void Adding(Action<T, TypeEvent> action, int index)
        {
            _index = index;
            action((T)this, TypeEvent.Add);
            _eventChanged.Add(action);
        }

        public Subscription Subscribe(Action<T, TypeEvent> action, bool instantGetValue = true)
        {
            if (instantGetValue)
                action((T)this, TypeEvent.Subscribe);
            return _eventChanged.Add(action);
        }
        public void Unsubscribe(Action<T, TypeEvent> action) => _eventChanged.Remove(action);
        public void UnsubscribeAll() => _eventChanged.Clear();

        public virtual void Removing()
        {
            _eventChanged.Invoke((T)this, TypeEvent.Remove);
            _eventChanged.Clear();
            _index = -1;
        }

        public abstract bool Equals(T other);
        public abstract void Dispose();
    }
}
