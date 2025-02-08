//Assets\Vurbiri\Runtime\Types\Reactive\Collections\Abstract\AReactiveElement.cs
using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive.Collections
{
    public abstract class AReactiveElement<T> : IReactiveElement<T> where T : AReactiveElement<T>
	{
        private readonly HashSet<Action<T, TypeEvent>> _hashActions = new();

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
            if (_hashActions.Count == 0)
                actionThisChange += RedirectEvent;

            _hashActions.Add(action);
            if (calling)
                action((T)this, TypeEvent.Subscribe);
            return new Unsubscriber<Action<T, TypeEvent>>(this, action);
        }

        public void Unsubscribe(Action<T, TypeEvent> action)
        {
            _hashActions.Remove(action);
            if(_hashActions.Count == 0)
                actionThisChange -= RedirectEvent;
        }

        public virtual void Removing()
        {
            actionThisChange?.Invoke((T)this, TypeEvent.Remove);
            actionThisChange = null;
            _hashActions.Clear();
            _index = -1;
        }

        private void RedirectEvent(T self, TypeEvent type)
        {
            foreach (var action in _hashActions)
                action(self, type);
        }

        public abstract bool Equals(T other);
    }
}
