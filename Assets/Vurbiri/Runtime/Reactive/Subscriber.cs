//Assets\Vurbiri\Runtime\Reactive\Subscriber.cs
using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive
{
    public class Subscriber : ISubscriber, IDisposable
    {
        protected Action actions;

        public Unsubscriber Add(Action action)
        {
            Errors.ThrowIfNull(action);
            
            actions -= action;
            actions += action;
            return new Unsubscriber<Action>(this, action);
        }

        public void Invoke() => actions?.Invoke();

        public void Remove(Action action) => actions -= action;

        public void Dispose() => actions = null;
    }
    //=======================================================================================

    public class Subscriber<T> : ISubscriber<T>, IDisposable
    {
        protected Action<T> actions;

        public Unsubscriber Add(Action<T> action)
        {
            Errors.ThrowIfNull(action);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<T>>(this, action);
        }

        public Unsubscriber Add(Action<T> action, bool calling, T value)
        {
            Errors.ThrowIfNull(action);

            if (calling) action(value);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<T>>(this, action);
        }

        public Unsubscriber Add(Action<T> action, bool calling, IReadOnlyList<T> values)
        {
            Errors.ThrowIfNull(action);

            if (calling)
            {
                for (int i = 0; i < values.Count; i++)
                    action(values[i]);
            }

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<T>>(this, action);
        }

        public Unsubscriber Add<U>(Action<T> action, bool calling, IReadOnlyList<U> values, Func<U,T> get)
        {
            Errors.ThrowIfNull(action);

            if (calling)
            {
                for (int i = 0; i < values.Count; i++)
                    action(get(values[i]));
            }

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<T>>(this, action);
        }

        public void Invoke(T value) => actions?.Invoke(value);

        public void Remove(Action<T> action) => actions -= action;

        public void Dispose() => actions = null;

        public static Unsubscriber operator +(Subscriber<T> subscriber, Action<T> action)
        {
            Errors.ThrowIfNull(action);

            subscriber.actions -= action;
            subscriber.actions += action;

            return new Unsubscriber<Action<T>>(subscriber, action);
        }
    }
    //=======================================================================================
    public class Subscriber<TA, TB> : ISubscriber<TA, TB>, IDisposable
    {
        protected Action<TA, TB> actions;

        public Unsubscriber Add(Action<TA, TB> action)
        {
            Errors.ThrowIfNull(action);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB>>(this, action);
        }

        public Unsubscriber Add(Action<TA, TB> action, bool calling, TA valueA, TB valueB)
        {
            Errors.ThrowIfNull(action);

            if (calling) action(valueA, valueB);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB>>(this, action);
        }

        public void Invoke(TA valueA, TB valueB) => actions?.Invoke(valueA, valueB);

        public void Remove(Action<TA, TB> action) => actions -= action;

        public void Dispose() => actions = null;
    }
    //=======================================================================================
    public class Subscriber<TA, TB, TC> : ISubscriber<TA, TB, TC>, IDisposable
    {
        protected Action<TA, TB, TC> actions;

        public Unsubscriber Add(Action<TA, TB, TC> action)
        {
            Errors.ThrowIfNull(action);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB, TC>>(this, action);
        }

        public Unsubscriber Add(Action<TA, TB, TC> action, bool calling, TA valueA, TB valueB, TC valueC)
        {
            Errors.ThrowIfNull(action);

            if (calling) action(valueA, valueB, valueC);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB, TC>>(this, action);
        }

        public void Invoke(TA valueA, TB valueB, TC valueC) => actions?.Invoke(valueA, valueB, valueC);

        public void Remove(Action<TA, TB, TC> action) => actions -= action;

        public void Dispose() => actions = null;
    }
    //=======================================================================================
    public class Subscriber<TA, TB, TC, TD> : ISubscriber<TA, TB, TC, TD>, IDisposable
    {
        protected Action<TA, TB, TC, TD> actions;

        public Unsubscriber Add(Action<TA, TB, TC, TD> action)
        {
            Errors.ThrowIfNull(action);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB, TC, TD>>(this, action);
        }

        public Unsubscriber Add(Action<TA, TB, TC, TD> action, bool calling, TA valueA, TB valueB, TC valueC, TD valueD)
        {
            Errors.ThrowIfNull(action);

            if (calling) action(valueA, valueB, valueC, valueD);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB, TC, TD>>(this, action);
        }

        public void Invoke(TA valueA, TB valueB, TC valueC, TD valueD) => actions?.Invoke(valueA, valueB, valueC, valueD);

        public void Remove(Action<TA, TB, TC, TD> action) => actions -= action;

        public void Dispose() => actions = null;
    }
}
