namespace Vurbiri.Reactive.Collections
{
    using System;

    public abstract class AReactiveElement<T> : IReactiveElement<T> where T : AReactiveElement<T>
	{
        protected Action<T, Operation> actionThisChange;
        protected int _index = -1;

        public int Index { get => _index; set => _index = value; }

        public void Subscribe(Action<T, Operation> action, int index)
        {
            actionThisChange -= action ?? throw new ArgumentNullException("action");
            actionThisChange += action;
            _index = index;
            action((T)this, Operation.Add);
        }
    }
}
