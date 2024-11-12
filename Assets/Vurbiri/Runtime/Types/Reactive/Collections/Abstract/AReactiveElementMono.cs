namespace Vurbiri.Reactive.Collections
{
    using System;
    using UnityEngine;

    public class AReactiveElementMono<T> : MonoBehaviour, IReactiveElement<T> where T : AReactiveElementMono<T>
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

        protected virtual void Removing()
        {
            actionThisChange?.Invoke((T)this, Operation.Remove);
            actionThisChange = null;
            _index = -1;
            Destroy(gameObject);
        }
    }
}
