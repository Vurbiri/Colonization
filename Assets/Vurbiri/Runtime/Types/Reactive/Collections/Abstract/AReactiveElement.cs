//Assets\Vurbiri\Runtime\Types\Reactive\Collections\Abstract\AReactiveElement.cs
namespace Vurbiri.Reactive.Collections
{
    using System;

    public abstract class AReactiveElement<T> : IReactiveElement<T> where T : AReactiveElement<T>
	{
        protected Action<T, TypeEvent> actionThisChange;
        protected int _index = -1;

        public int Index { get => _index; set => _index = value; }

        public void Subscribe(Action<T, TypeEvent> action, int index)
        {
            actionThisChange -= action ?? throw new ArgumentNullException("action");
            actionThisChange += action;
            _index = index;
            action((T)this, TypeEvent.Subscribe);
        }

        public virtual void Removing()
        {
            actionThisChange?.Invoke((T)this, TypeEvent.Remove);
            actionThisChange = null;
            _index = -1;
        }
    }
}
