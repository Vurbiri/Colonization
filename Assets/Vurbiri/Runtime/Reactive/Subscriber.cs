//Assets\Vurbiri\Runtime\Reactive\Subscriber.cs
using System;

namespace Vurbiri.Reactive
{
    public struct Subscriber<T> : ISubscriber<Action<T>>, IDisposable
    {
        private Action<T> actions;

        public Unsubscriber Add(Action<T> action)
        {
            actions -= action;
            actions += action;
            return new Unsubscriber<Action<T>>(this, action);
        }

        public readonly void Invoke(T value) => actions?.Invoke(value);

        public void Unsubscribe(Action<T> action) => actions -= action;

        public void Dispose() => actions = null;
    }
    //=======================================================================================
    public struct Subscriber<TA, TB> : ISubscriber<Action<TA, TB>>, IDisposable
    {
        private Action<TA, TB> actions;

        public Unsubscriber Add(Action<TA, TB> action)
        {
            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB>>(this, action);
        }

        public readonly void Invoke(TA valueA, TB valueB) => actions?.Invoke(valueA, valueB);

        public void Unsubscribe(Action<TA, TB> action) => actions -= action;

        public void Dispose() => actions = null;
    }
    //=======================================================================================
    public struct Subscriber<TA, TB, TC> : ISubscriber<Action<TA, TB, TC>>, IDisposable
    {
        private Action<TA, TB, TC> actions;

        public Unsubscriber Add(Action<TA, TB, TC> action)
        {
            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB, TC>>(this, action);
        }

        public readonly void Invoke(TA valueA, TB valueB, TC valueC) => actions?.Invoke(valueA, valueB, valueC);

        public void Unsubscribe(Action<TA, TB, TC> action) => actions -= action;

        public void Dispose() => actions = null;
    }
    //=======================================================================================
    public struct Subscriber<TA, TB, TC, TD> : ISubscriber<Action<TA, TB, TC, TD>>, IDisposable
    {
        private Action<TA, TB, TC, TD> actions;

        public Unsubscriber Add(Action<TA, TB, TC, TD> action)
        {
            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB, TC, TD>>(this, action);
        }

        public readonly void Invoke(TA valueA, TB valueB, TC valueC, TD valueD) => actions?.Invoke(valueA, valueB, valueC, valueD);

        public void Unsubscribe(Action<TA, TB, TC, TD> action) => actions -= action;

        public void Dispose() => actions = null;
    }
}
