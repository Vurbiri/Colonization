//Assets\Vurbiri\Runtime\Reactive\Subscriber.cs
using System;

namespace Vurbiri.Reactive
{
    public class Subscriber<T> : ISubscriber<Action<T>>, IDisposable
    {
        private Action<T> actions;

        public IUnsubscriber Add(Action<T> action)
        {
            actions += action;
            return new Unsubscriber<Action<T>>(this, action);
        }

        public void Invoke(T value) => actions?.Invoke(value);

        public void Unsubscribe(Action<T> action) => actions -= action;

        public void Dispose() => actions = null;
    }
    //=======================================================================================
    public class Subscriber<TA, TB> : ISubscriber<Action<TA, TB>>, IDisposable
    {
        private Action<TA, TB> actions;

        public IUnsubscriber Add(Action<TA, TB> action)
        {
            actions += action;
            return new Unsubscriber<Action<TA, TB>>(this, action);
        }

        public void Invoke(TA valueA, TB valueB) => actions?.Invoke(valueA, valueB);

        public void Unsubscribe(Action<TA, TB> action) => actions -= action;

        public void Dispose() => actions = null;
    }
    //=======================================================================================
    public class Subscriber<TA, TB, TC> : ISubscriber<Action<TA, TB, TC>>, IDisposable
    {
        private Action<TA, TB, TC> actions;

        public IUnsubscriber Add(Action<TA, TB, TC> action)
        {
            actions += action;
            return new Unsubscriber<Action<TA, TB, TC>>(this, action);
        }

        public void Invoke(TA valueA, TB valueB, TC valueC) => actions?.Invoke(valueA, valueB, valueC);

        public void Unsubscribe(Action<TA, TB, TC> action) => actions -= action;

        public void Dispose() => actions = null;
    }
    //=======================================================================================
    public class Subscriber<TA, TB, TC, TD> : ISubscriber<Action<TA, TB, TC, TD>>, IDisposable
    {
        private Action<TA, TB, TC, TD> actions;

        public IUnsubscriber Add(Action<TA, TB, TC, TD> action)
        {
            actions += action;
            return new Unsubscriber<Action<TA, TB, TC, TD>>(this, action);
        }

        public void Invoke(TA valueA, TB valueB, TC valueC, TD valueD) => actions?.Invoke(valueA, valueB, valueC, valueD);

        public void Unsubscribe(Action<TA, TB, TC, TD> action) => actions -= action;

        public void Dispose() => actions = null;
    }
}
