//Assets\Vurbiri\Runtime\Types\Reactive\Collections\Abstract\AReactiveElement.cs
using System;
using System.Collections.Generic;


namespace Vurbiri.Reactive.Collections
{
    public abstract class AReactiveElement<T> : IReactiveElement<T> where T : AReactiveElement<T>
	{
        private readonly HashSet<Action<TypeEvent>> _hashActions = new();

        protected Action<T, TypeEvent> actionThisChange;
        protected int _index = -1;

        public int Index { get => _index; set => _index = value; }

        public void Subscribe(Action<T, TypeEvent> action, int index)
        {
            actionThisChange += action;
            _index = index;
            action((T)this, TypeEvent.Subscribe);
        }

        public IUnsubscriber Subscribe(Action<TypeEvent> action, bool calling = true)
        {
            if (_hashActions.Count == 0)
                actionThisChange += RedirectEvent;

            _hashActions.Add(action);
            if (calling)
                action(TypeEvent.Subscribe);
            return new Unsubscriber<Action<TypeEvent>>(this, action);
        }

        public void Unsubscribe(Action<TypeEvent> action)
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
                action(type);
        }
    }
}
