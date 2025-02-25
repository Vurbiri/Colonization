//Assets\Vurbiri\Runtime\Reactive\Subscriber.cs
using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive
{
    public class Subscriber<T> : ISubscriber<Action<T>>, IDisposable
    {
        internal readonly HashSet<Unsubscriber<Action<T>>> _actions = new();
        private Action<T> actions;

        public IUnsubscriber Add(Action<T> action)
        {
            actions += action;

            Unsubscriber<Action<T>> temp = new(this, action);
            _actions.Add(temp);
            return temp;
        }

        public void Invoke(T value) => actions?.Invoke(value);

        public void Dispose()
        {
            foreach (var unsubscriber in _actions)
                unsubscriber.Clear();

            actions = null;
            _actions.Clear();
        }

        void ISubscriber<Action<T>>.Unsubscribe(Unsubscriber<Action<T>> unsubscriber)
        {
            _actions.Remove(unsubscriber);
            actions -= unsubscriber.action;
        }
    }

    public class Subscriber<TA, TB> : ISubscriber<Action<TA, TB>>, IDisposable
    {
        internal readonly HashSet<Unsubscriber<Action<TA, TB>>> _actions = new();
        private Action<TA, TB> actions;

        public IUnsubscriber Add(Action<TA, TB> action)
        {
            actions += action;

            Unsubscriber<Action<TA, TB>> temp = new(this, action);
            _actions.Add(temp);
            return temp;
        }

        public void Invoke(TA valueA, TB valueB) => actions?.Invoke(valueA, valueB);

        public void Dispose()
        {
            foreach (var unsubscriber in _actions)
                unsubscriber.Clear();

            actions = null;
            _actions.Clear();
        }

        void ISubscriber<Action<TA, TB>>.Unsubscribe(Unsubscriber<Action<TA, TB>> unsubscriber)
        {
            _actions.Remove(unsubscriber);
            actions -= unsubscriber.action;
        }
    }

    public class Subscriber<TA, TB, TC> : ISubscriber<Action<TA, TB, TC>>, IDisposable
    {
        internal readonly HashSet<Unsubscriber<Action<TA, TB, TC>>> _actions = new();
        private Action<TA, TB, TC> actions;

        public IUnsubscriber Add(Action<TA, TB, TC> action)
        {
            actions += action;

            Unsubscriber<Action<TA, TB, TC>> temp = new(this, action);
            _actions.Add(temp);
            return temp;
        }

        public void Invoke(TA valueA, TB valueB, TC valueC) => actions?.Invoke(valueA, valueB, valueC);

        public void Dispose()
        {
            foreach (var unsubscriber in _actions)
                unsubscriber.Clear();

            actions = null;
            _actions.Clear();
        }

        void ISubscriber<Action<TA, TB, TC>>.Unsubscribe(Unsubscriber<Action<TA, TB, TC>> unsubscriber)
        {
            _actions.Remove(unsubscriber);
            actions -= unsubscriber.action;
        }
    }
}
