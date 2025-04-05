//Assets\Vurbiri\Runtime\Reactive\Signer.cs
using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive
{
    public class Signer : ISigner
    {
        protected Action actions;

        public Unsubscriber Add(Action action)
        {
            Throw.IfNull(action);
            
            actions -= action;
            actions += action;
            return new Unsubscriber<Action>(this, action);
        }

        public void Invoke() => actions?.Invoke();

        public void Remove(Action action) => actions -= action;
    }
    //=======================================================================================

    public class Signer<T> : ISigner<T>
    {
        protected Action<T> actions;

        public Unsubscriber Add(Action<T> action)
        {
            Throw.IfNull(action);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<T>>(this, action);
        }

        public Unsubscriber Add(Action<T> action, bool instantGetValue, T value)
        {
            Throw.IfNull(action);

            if (instantGetValue) action(value);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<T>>(this, action);
        }

        public Unsubscriber Add(Action<T> action, bool instantGetValue, IReadOnlyList<T> values)
        {
            Throw.IfNull(action);

            if (instantGetValue)
            {
                int count = values.Count;
                for (int i = 0; i < count; i++)
                    action(values[i]);
            }

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<T>>(this, action);
        }

        public Unsubscriber Add<U>(Action<T> action, bool instantGetValue, IReadOnlyList<U> values, Func<U,T> get)
        {
            Throw.IfNull(action);

            if (instantGetValue)
            {
                int count = values.Count;
                for (int i = 0; i < count; i++)
                    action(get(values[i]));
            }

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<T>>(this, action);
        }

        public void Invoke(T value) => actions?.Invoke(value);

        public void Remove(Action<T> action) => actions -= action;

        public static Unsubscriber operator +(Signer<T> subscriber, Action<T> action)
        {
            Throw.IfNull(action);

            subscriber.actions -= action;
            subscriber.actions += action;

            return new Unsubscriber<Action<T>>(subscriber, action);
        }
    }
    //=======================================================================================
    public class Signer<TA, TB> : ISigner<TA, TB>
    {
        protected Action<TA, TB> actions;

        public Unsubscriber Add(Action<TA, TB> action)
        {
            Throw.IfNull(action);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB>>(this, action);
        }

        public Unsubscriber Add(Action<TA, TB> action, bool instantGetValue, TA valueA, TB valueB)
        {
            Throw.IfNull(action);

            if (instantGetValue) action(valueA, valueB);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB>>(this, action);
        }

        public void Invoke(TA valueA, TB valueB) => actions?.Invoke(valueA, valueB);

        public void Remove(Action<TA, TB> action) => actions -= action;
    }
    //=======================================================================================
    public class Signer<TA, TB, TC> : ISigner<TA, TB, TC>
    {
        protected Action<TA, TB, TC> actions;

        public Unsubscriber Add(Action<TA, TB, TC> action)
        {
            Throw.IfNull(action);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB, TC>>(this, action);
        }

        public Unsubscriber Add(Action<TA, TB, TC> action, bool instantGetValue, TA valueA, TB valueB, TC valueC)
        {
            Throw.IfNull(action);

            if (instantGetValue) action(valueA, valueB, valueC);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB, TC>>(this, action);
        }

        public void Invoke(TA valueA, TB valueB, TC valueC) => actions?.Invoke(valueA, valueB, valueC);

        public void Remove(Action<TA, TB, TC> action) => actions -= action;
    }
    //=======================================================================================
    public class Signer<TA, TB, TC, TD> : ISigner<TA, TB, TC, TD>
    {
        protected Action<TA, TB, TC, TD> actions;

        public Unsubscriber Add(Action<TA, TB, TC, TD> action)
        {
            Throw.IfNull(action);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB, TC, TD>>(this, action);
        }

        public Unsubscriber Add(Action<TA, TB, TC, TD> action, bool instantGetValue, TA valueA, TB valueB, TC valueC, TD valueD)
        {
            Throw.IfNull(action);

            if (instantGetValue) action(valueA, valueB, valueC, valueD);

            actions -= action;
            actions += action;
            return new Unsubscriber<Action<TA, TB, TC, TD>>(this, action);
        }

        public void Invoke(TA valueA, TB valueB, TC valueC, TD valueD) => actions?.Invoke(valueA, valueB, valueC, valueD);

        public void Remove(Action<TA, TB, TC, TD> action) => actions -= action;
    }
}
