//Assets\Vurbiri\Runtime\Reactive\Collections\Abstract\AReactiveItemMono.cs
using System;
using UnityEngine;

namespace Vurbiri.Reactive.Collections
{
    public abstract class AReactiveItemMono<T> : MonoBehaviour, IReactiveItem<T> where T : AReactiveItemMono<T>
    {
        protected readonly Subscription<T, TypeEvent> _eventChanged = new();
        protected int _index = -1;

        public int Index { get => _index; set { _index = value; _eventChanged.Invoke((T)this, TypeEvent.Reindex); } }

        public void Adding(Action<T, TypeEvent> action, int index)
        {
            _index = index;
            action((T)this, TypeEvent.Add);
            _eventChanged.Add(action);
        }

        public Unsubscription Subscribe(Action<T, TypeEvent> action, bool instantGetValue = true)
        {
            if (instantGetValue)
                action((T)this, TypeEvent.Subscribe);
            return _eventChanged.Add(action);
        }

        public virtual void Removing()
        {
            _eventChanged.Invoke((T)this, TypeEvent.Remove);
            _index = -1;
        }

        public abstract bool Equals(T other);
        public abstract void Dispose();
    }
}
