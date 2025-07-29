using System;
using System.Collections.Generic;

namespace Vurbiri.Reactive
{
    public class Subscription : ISubscription
    {
        protected Action a_delegate;

        public Subscription() => a_delegate = Empty;

        public Unsubscription Add(Action action)
        {
            Throw.IfNull(action, "action");
            
            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action>(this, action);
        }

        public void Invoke() => a_delegate();

        public void Remove(Action action) => a_delegate -= action;
        public void Clear() => a_delegate = Empty;

        private static void Empty() { }
    }
    //=======================================================================================

    public class Subscription<T> : ISubscription<T>
    {
        protected Action<T> a_delegate;

        public Subscription() => a_delegate = Empty;

        public Unsubscription Add(Action<T> action)
        {
            Throw.IfNull(action, "action");

            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action<T>>(this, action);
        }

        public Unsubscription Add(Action<T> action, T value)
        {
            Throw.IfNull(action);

            action(value);

            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action<T>>(this, action);
        }

        public Unsubscription Add(Action<T> action, bool instantGetValue, T value)
        {
            Throw.IfNull(action, "action");

            if (instantGetValue) action(value);

            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action<T>>(this, action);
        }

        public Unsubscription Add(Action<T> action, IReadOnlyList<T> values)
        {
            Throw.IfNull(action, "action");

            int count = values.Count;
            for (int i = 0; i < count; i++)
                action(values[i]);

            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action<T>>(this, action);
        }

        public Unsubscription Add(Action<T> action, bool instantGetValue, IReadOnlyList<T> values)
        {
            Throw.IfNull(action, "action");

            if (instantGetValue)
            {
                int count = values.Count;
                for (int i = 0; i < count; i++)
                    action(values[i]);
            }

            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action<T>>(this, action);
        }

        public Unsubscription Add<U>(Action<T> action, IReadOnlyList<U> values, Func<U, T> get)
        {
            Throw.IfNull(action, "action");

            int count = values.Count;
            for (int i = 0; i < count; i++)
                action(get(values[i]));

            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action<T>>(this, action);
        }

        public Unsubscription Add<U>(Action<T> action, bool instantGetValue, IReadOnlyList<U> values, Func<U,T> get)
        {
            Throw.IfNull(action);

            if (instantGetValue)
            {
                int count = values.Count;
                for (int i = 0; i < count; i++)
                    action(get(values[i]));
            }

            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action<T>>(this, action);
        }

        public void Invoke(T value) => a_delegate.Invoke(value);

        public void Remove(Action<T> action) => a_delegate -= action;
        public void Clear() => a_delegate = Empty;

        private static void Empty(T t) { }
    }
    //=======================================================================================
    public class Subscription<TA, TB> : ISubscription<TA, TB>
    {
        protected Action<TA, TB> a_delegate;

        public Subscription() => a_delegate = Empty;

        public Unsubscription Add(Action<TA, TB> action)
        {
            Throw.IfNull(action, "action");

            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action<TA, TB>>(this, action);
        }

        public Unsubscription Add(Action<TA, TB> action, TA valueA, TB valueB)
        {
            Throw.IfNull(action, "action");

            action(valueA, valueB);

            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action<TA, TB>>(this, action);
        }

        public Unsubscription Add(Action<TA, TB> action, bool instantGetValue, TA valueA, TB valueB)
        {
            Throw.IfNull(action, "action");

            if (instantGetValue) action(valueA, valueB);

            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action<TA, TB>>(this, action);
        }

        public void Invoke(TA valueA, TB valueB) => a_delegate.Invoke(valueA, valueB);

        public void Remove(Action<TA, TB> action) => a_delegate -= action;
        public void Clear() => a_delegate = Empty;

        private static void Empty(TA ta, TB tb) { }
    }
    //=======================================================================================
    public class Subscription<TA, TB, TC> : ISubscription<TA, TB, TC>
    {
        protected Action<TA, TB, TC> a_delegate;

        public Subscription() => a_delegate = Empty;

        public Unsubscription Add(Action<TA, TB, TC> action)
        {
            Throw.IfNull(action, "action");

            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action<TA, TB, TC>>(this, action);
        }

        public Unsubscription Add(Action<TA, TB, TC> action, TA valueA, TB valueB, TC valueC)
        {
            Throw.IfNull(action, "action");

            action(valueA, valueB, valueC);

            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action<TA, TB, TC>>(this, action);
        }

        public Unsubscription Add(Action<TA, TB, TC> action, bool instantGetValue, TA valueA, TB valueB, TC valueC)
        {
            Throw.IfNull(action, "action");

            if (instantGetValue) action(valueA, valueB, valueC);

            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action<TA, TB, TC>>(this, action);
        }

        public void Invoke(TA valueA, TB valueB, TC valueC) => a_delegate.Invoke(valueA, valueB, valueC);

        public void Remove(Action<TA, TB, TC> action) => a_delegate -= action;
        public void Clear() => a_delegate = Empty;

        private static void Empty(TA ta, TB tb, TC tc) { }
    }
    //=======================================================================================
    public class Subscription<TA, TB, TC, TD> : ISubscription<TA, TB, TC, TD>
    {
        protected Action<TA, TB, TC, TD> a_delegate;

        public Subscription() => a_delegate = Empty;

        public Unsubscription Add(Action<TA, TB, TC, TD> action)
        {
            Throw.IfNull(action, "action");

            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action<TA, TB, TC, TD>>(this, action);
        }

        public Unsubscription Add(Action<TA, TB, TC, TD> action, TA valueA, TB valueB, TC valueC, TD valueD)
        {
            Throw.IfNull(action, "action");

            action(valueA, valueB, valueC, valueD);

            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action<TA, TB, TC, TD>>(this, action);
        }

        public Unsubscription Add(Action<TA, TB, TC, TD> action, bool instantGetValue, TA valueA, TB valueB, TC valueC, TD valueD)
        {
            Throw.IfNull(action, "action");

            if (instantGetValue) action(valueA, valueB, valueC, valueD);

            a_delegate -= action;
            a_delegate += action;
            return new Unsubscription<Action<TA, TB, TC, TD>>(this, action);
        }

        public void Invoke(TA valueA, TB valueB, TC valueC, TD valueD) => a_delegate.Invoke(valueA, valueB, valueC, valueD);

        public void Remove(Action<TA, TB, TC, TD> action) => a_delegate -= action;
        public void Clear() => a_delegate = Empty;

        private static void Empty(TA ta, TB tb, TC tc, TD td) { }
    }
}
