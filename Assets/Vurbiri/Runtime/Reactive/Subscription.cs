using System;

namespace Vurbiri.Reactive
{
    public abstract class Event : IUnsubscribed<Action>
    {
        protected Action a_delegate;

        public Unsubscription Add(Action action)
        {
            a_delegate += action;
            return new Unsubscription<Action>(this, action);
        }

        public void Remove(Action action) => a_delegate -= action;

        public static Unsubscription operator +(Event self, Action action) => self.Add(action);
        public static Event operator -(Event self, Action action)
        {
            self.a_delegate -= action;
            return self;
        }
    }
    //----------------------------------------------
    public class Subscription : Event
    {
        public Subscription() => a_delegate = Dummy;

        public void Invoke() => a_delegate();
        public void InvokeOneShot()
        {
            a_delegate();
            a_delegate = Dummy;
        }

        public void Clear() => a_delegate = Dummy;

        private static void Dummy() { }
    }
    //=======================================================================================
    public abstract class Event<T> : IUnsubscribed<Action<T>>
    {
        protected Action<T> a_delegate;

        public Unsubscription Add(Action<T> action)
        {
            a_delegate += action;
            return new Unsubscription<Action<T>>(this, action);
        }

        public Unsubscription Add(Action<T> action, T value)
        {
            action(value);

            a_delegate += action;
            return new Unsubscription<Action<T>>(this, action);
        }

        public Unsubscription Add(Action<T> action, bool instantGetValue, T value)
        {
            if (instantGetValue) action(value);

            a_delegate += action;
            return new Unsubscription<Action<T>>(this, action);
        }

        public void Remove(Action<T> action) => a_delegate -= action;

        public static Unsubscription operator +(Event<T> self, Action<T> action) => self.Add(action);
        public static Event<T> operator -(Event<T> self, Action<T> action)
        {
            self.a_delegate -= action;
            return self;
        }
    }
    //----------------------------------------------
    public class Subscription<T> : Event<T>
    {
        public Subscription() => a_delegate = Dummy;

        public void Invoke(T value) => a_delegate.Invoke(value);
        public void InvokeOneShot(T value)
        {
            a_delegate.Invoke(value);
            a_delegate = Dummy;
        }

        public void Clear() => a_delegate = Dummy;

        private static void Dummy(T t) { }
    }
    //=======================================================================================
    public abstract class Event<TA, TB> : IUnsubscribed<Action<TA, TB>>
    {
        protected Action<TA, TB> a_delegate;

        public Unsubscription Add(Action<TA, TB> action)
        {
            a_delegate += action;
            return new Unsubscription<Action<TA, TB>>(this, action);
        }

        public Unsubscription Add(Action<TA, TB> action, TA valueA, TB valueB)
        {
            action(valueA, valueB);

            a_delegate += action;
            return new Unsubscription<Action<TA, TB>>(this, action);
        }

        public Unsubscription Add(Action<TA, TB> action, bool instantGetValue, TA valueA, TB valueB)
        {
            if (instantGetValue) action(valueA, valueB);

            a_delegate += action;
            return new Unsubscription<Action<TA, TB>>(this, action);
        }

        public void Remove(Action<TA, TB> action) => a_delegate -= action;

        public static Unsubscription operator +(Event<TA, TB> self, Action<TA, TB> action) => self.Add(action);
        public static Event<TA, TB> operator -(Event<TA, TB> self, Action<TA, TB> action)
        {
            self.Remove(action);
            return self;
        }
    }
    //----------------------------------------------
    public class Subscription<TA, TB> : Event<TA, TB>
    {
        public Subscription() => a_delegate = Dummy;

        public void Invoke(TA valueA, TB valueB) => a_delegate.Invoke(valueA, valueB);
        public void InvokeOneShot(TA valueA, TB valueB)
        {
            a_delegate.Invoke(valueA, valueB);
            a_delegate = Dummy;
        }

        public void Clear() => a_delegate = Dummy;

        private static void Dummy(TA ta, TB tb) { }
    }
    //=======================================================================================
    public abstract class Event<TA, TB, TC> : IUnsubscribed<Action<TA, TB, TC>>
    {
        protected Action<TA, TB, TC> a_delegate;

        public Unsubscription Add(Action<TA, TB, TC> action)
        {
            a_delegate += action;
            return new Unsubscription<Action<TA, TB, TC>>(this, action);
        }

        public Unsubscription Add(Action<TA, TB, TC> action, TA valueA, TB valueB, TC valueC)
        {
            action(valueA, valueB, valueC);

            a_delegate += action;
            return new Unsubscription<Action<TA, TB, TC>>(this, action);
        }

        public Unsubscription Add(Action<TA, TB, TC> action, bool instantGetValue, TA valueA, TB valueB, TC valueC)
        {
            if (instantGetValue) action(valueA, valueB, valueC);

            a_delegate += action;
            return new Unsubscription<Action<TA, TB, TC>>(this, action);
        }

        public void Remove(Action<TA, TB, TC> action) => a_delegate -= action;

        public static Unsubscription operator +(Event<TA, TB, TC> self, Action<TA, TB, TC> action) => self.Add(action);
        public static Event<TA, TB, TC> operator -(Event<TA, TB, TC> self, Action<TA, TB, TC> action)
        {
            self.Remove(action);
            return self;
        }
    }
    //----------------------------------------------
    public class Subscription<TA, TB, TC> : Event<TA, TB, TC>
    {
        public Subscription() => a_delegate = Dummy;

        public void Invoke(TA valueA, TB valueB, TC valueC) => a_delegate.Invoke(valueA, valueB, valueC);
        public void InvokeOneShot(TA valueA, TB valueB, TC valueC)
        {
            a_delegate.Invoke(valueA, valueB, valueC);
            a_delegate = Dummy;
        }

        public void Clear() => a_delegate = Dummy;

        private static void Dummy(TA ta, TB tb, TC tc) { }
    }
    //=======================================================================================
    public abstract class Event<TA, TB, TC, TD> : IUnsubscribed<Action<TA, TB, TC, TD>>
    {
        protected Action<TA, TB, TC, TD> a_delegate;

        public Unsubscription Add(Action<TA, TB, TC, TD> action)
        {
            a_delegate += action;
            return new Unsubscription<Action<TA, TB, TC, TD>>(this, action);
        }

        public Unsubscription Add(Action<TA, TB, TC, TD> action, TA valueA, TB valueB, TC valueC, TD valueD)
        {
            action(valueA, valueB, valueC, valueD);

            a_delegate += action;
            return new Unsubscription<Action<TA, TB, TC, TD>>(this, action);
        }

        public Unsubscription Add(Action<TA, TB, TC, TD> action, bool instantGetValue, TA valueA, TB valueB, TC valueC, TD valueD)
        {
            if (instantGetValue) action(valueA, valueB, valueC, valueD);

            a_delegate += action;
            return new Unsubscription<Action<TA, TB, TC, TD>>(this, action);
        }

        public void Remove(Action<TA, TB, TC, TD> action) => a_delegate -= action;

        public static Unsubscription operator +(Event<TA, TB, TC, TD> self, Action<TA, TB, TC, TD> action) => self.Add(action);
        public static Event<TA, TB, TC, TD> operator -(Event<TA, TB, TC, TD> self, Action<TA, TB, TC, TD> action)
        {
            self.Remove(action);
            return self;
        }
    }
    //----------------------------------------------
    public class Subscription<TA, TB, TC, TD> : Event<TA, TB, TC, TD>
    {
        public Subscription() => a_delegate = Dummy;

        public void Invoke(TA valueA, TB valueB, TC valueC, TD valueD) => a_delegate.Invoke(valueA, valueB, valueC, valueD);
        public void InvokeOneShot(TA valueA, TB valueB, TC valueC, TD valueD)
        {
            a_delegate.Invoke(valueA, valueB, valueC, valueD);
            a_delegate = Dummy;
        }

        public void Clear() => a_delegate = Dummy;

        private static void Dummy(TA ta, TB tb, TC tc, TD td) { }
    }
}
