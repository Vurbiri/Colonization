//Assets\Vurbiri\Runtime\Reactive\Subscriber.cs
using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive
{
    public class Subscriber<T> : ISubscriber<Action<T>>, IDisposable
    {
        private Action<T> actions;

        public Unsubscriber Add(Action<T> action)
        {
            Errors.CheckForNull(action);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<T>>(this, action);
        }

        public Unsubscriber Add(Action<T> action, bool calling, T value)
        {
            Errors.CheckForNull(action);

            if (calling) action(value);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<T>>(this, action);
        }

        public Unsubscriber Add(Action<T> action, bool calling, IReadOnlyList<T> values)
        {
            Errors.CheckForNull(action);

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
            Errors.CheckForNull(action);

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

        public void Unsubscribe(Action<T> action) => actions -= action;

        public void Dispose() => actions = null;

        public static Unsubscriber operator +(Subscriber<T> subscriber, Action<T> action)
        {
            Errors.CheckForNull(action);

            subscriber.actions -= action;
            subscriber.actions += action;

            return new Unsubscriber<Action<T>>(subscriber, action);
        }
    }
    //=======================================================================================
    public class Subscriber<TA, TB> : ISubscriber<Action<TA, TB>>, IDisposable
    {
        private Action<TA, TB> actions;

        public Unsubscriber Add(Action<TA, TB> action)
        {
            Errors.CheckForNull(action);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB>>(this, action);
        }

        public Unsubscriber Add(Action<TA, TB> action, bool calling, TA valueA, TB valueB)
        {
            Errors.CheckForNull(action);

            if (calling) action(valueA, valueB);

            actions -= action;
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

        public Unsubscriber Add(Action<TA, TB, TC> action)
        {
            Errors.CheckForNull(action);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB, TC>>(this, action);
        }

        public Unsubscriber Add(Action<TA, TB, TC> action, bool calling, TA valueA, TB valueB, TC valueC)
        {
            Errors.CheckForNull(action);

            if (calling) action(valueA, valueB, valueC);

            actions -= action;
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

        public Unsubscriber Add(Action<TA, TB, TC, TD> action)
        {
            Errors.CheckForNull(action);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB, TC, TD>>(this, action);
        }

        public Unsubscriber Add(Action<TA, TB, TC, TD> action, bool calling, TA valueA, TB valueB, TC valueC, TD valueD)
        {
            Errors.CheckForNull(action);

            if (calling) action(valueA, valueB, valueC, valueD);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB, TC, TD>>(this, action);
        }

        public void Invoke(TA valueA, TB valueB, TC valueC, TD valueD) => actions?.Invoke(valueA, valueB, valueC, valueD);

        public void Unsubscribe(Action<TA, TB, TC, TD> action) => actions -= action;

        public void Dispose() => actions = null;
    }
}
