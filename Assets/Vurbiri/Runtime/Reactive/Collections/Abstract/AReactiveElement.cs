//Assets\Vurbiri\Runtime\Types\Reactive\Collections\Abstract\AReactiveElement.cs
using System;

namespace Vurbiri.Reactive.Collections
{
    public abstract class AReactiveElement<T> : IReactiveElement<T> where T : AReactiveElement<T>
	{
        protected Action<T, TypeEvent> actionThisChange;
        protected int _index = -1;

        public int Index { get => _index; set { _index = value; actionThisChange?.Invoke((T)this, TypeEvent.Reindex); } }

        public void Adding(Action<T, TypeEvent> action, int index)
        {
            actionThisChange += action;
            _index = index;
            action((T)this, TypeEvent.Add);
        }

        public IUnsubscriber Subscribe(Action<T, TypeEvent> action, bool calling = true)
        {
            actionThisChange += action;

            if (calling)
                action((T)this, TypeEvent.Subscribe);
            return new Unsubscriber<Action<T, TypeEvent>>(this, action);
        }

        public void Unsubscribe(Action<T, TypeEvent> action)
        {
            actionThisChange -= action;
        }

        public virtual void Removing()
        {
            actionThisChange?.Invoke((T)this, TypeEvent.Remove);
            actionThisChange = null;
            _index = -1;
        }

        public abstract bool Equals(T other);
    }
}
