using System;

namespace Vurbiri.Reactive
{
    public abstract class Event : IUnsubscribed<Action>
    {
        protected Action _action;

        public Unsubscription Add(Action action)
        {
            _action += action;
            return Unsubscription.Create(this, action);
        }

        public void Remove(Action action) => _action -= action;

        public static Unsubscription operator +(Event self, Action action) => self.Add(action);
        public static Event operator -(Event self, Action action)
        {
            self._action -= action;
            return self;
        }
    }
    //----------------------------------------------
    public class Subscription : Event
    {
        public Subscription() => _action = Dummy;

        public void Invoke() => _action();

        public void Clear() => _action = Dummy;

        private static void Dummy() { }
    }
    //=======================================================================================
    public abstract class Event<T> : IUnsubscribed<Action<T>>
    {
        protected Action<T> _action;

        public Unsubscription Add(Action<T> action)
        {
            _action += action;
            return Unsubscription.Create(this, action);
        }

        public Unsubscription Add(Action<T> action, T value)
        {
            action(value);

            _action += action;
            return Unsubscription.Create(this, action);
        }

        public Unsubscription Add(Action<T> action, bool instantGetValue, T value)
        {
            if (instantGetValue) action(value);

            _action += action;
            return Unsubscription.Create(this, action);
        }

        public void Remove(Action<T> action) => _action -= action;

        public static Unsubscription operator +(Event<T> self, Action<T> action) => self.Add(action);
        public static Event<T> operator -(Event<T> self, Action<T> action)
        {
            self._action -= action;
            return self;
        }
    }
    //----------------------------------------------
    public class Subscription<T> : Event<T>
    {
        public Subscription() => _action = Dummy;

        public void Invoke(T value) => _action.Invoke(value);

        public void Clear() => _action = Dummy;

        private static void Dummy(T t) { }
    }
    //=======================================================================================
    public abstract class Event<TA, TB> : IUnsubscribed<Action<TA, TB>>
    {
        protected Action<TA, TB> _action;

        public Unsubscription Add(Action<TA, TB> action)
        {
            _action += action;
            return Unsubscription.Create(this, action);
        }

        public Unsubscription Add(Action<TA, TB> action, TA valueA, TB valueB)
        {
            action(valueA, valueB);

            _action += action;
            return Unsubscription.Create(this, action);
        }

        public Unsubscription Add(Action<TA, TB> action, bool instantGetValue, TA valueA, TB valueB)
        {
            if (instantGetValue) action(valueA, valueB);

            _action += action;
            return Unsubscription.Create(this, action);
        }

        public void Remove(Action<TA, TB> action) => _action -= action;

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
        public Subscription() => _action = Dummy;

        public void Invoke(TA valueA, TB valueB) => _action.Invoke(valueA, valueB);

        public void Clear() => _action = Dummy;

        private static void Dummy(TA ta, TB tb) { }
    }
    //=======================================================================================
    public abstract class Event<TA, TB, TC> : IUnsubscribed<Action<TA, TB, TC>>
    {
        protected Action<TA, TB, TC> _action;

        public Unsubscription Add(Action<TA, TB, TC> action)
        {
            _action += action;
            return Unsubscription.Create(this, action);
        }

        public Unsubscription Add(Action<TA, TB, TC> action, TA valueA, TB valueB, TC valueC)
        {
            action(valueA, valueB, valueC);

            _action += action;
            return Unsubscription.Create(this, action);
        }

        public Unsubscription Add(Action<TA, TB, TC> action, bool instantGetValue, TA valueA, TB valueB, TC valueC)
        {
            if (instantGetValue) action(valueA, valueB, valueC);

            _action += action;
            return Unsubscription.Create(this, action);
        }

        public void Remove(Action<TA, TB, TC> action) => _action -= action;

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
        public Subscription() => _action = Dummy;

        public void Invoke(TA valueA, TB valueB, TC valueC) => _action.Invoke(valueA, valueB, valueC);

        public void Clear() => _action = Dummy;

        private static void Dummy(TA ta, TB tb, TC tc) { }
    }
    //=======================================================================================
    public abstract class Event<TA, TB, TC, TD> : IUnsubscribed<Action<TA, TB, TC, TD>>
    {
        protected Action<TA, TB, TC, TD> _action;

        public Unsubscription Add(Action<TA, TB, TC, TD> action)
        {
            _action += action;
            return Unsubscription.Create(this, action);
        }

        public Unsubscription Add(Action<TA, TB, TC, TD> action, TA valueA, TB valueB, TC valueC, TD valueD)
        {
            action(valueA, valueB, valueC, valueD);

            _action += action;
            return Unsubscription.Create(this, action);
        }

        public Unsubscription Add(Action<TA, TB, TC, TD> action, bool instantGetValue, TA valueA, TB valueB, TC valueC, TD valueD)
        {
            if (instantGetValue) action(valueA, valueB, valueC, valueD);

            _action += action;
            return Unsubscription.Create(this, action);
        }

        public void Remove(Action<TA, TB, TC, TD> action) => _action -= action;

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
        public Subscription() => _action = Dummy;

        public void Invoke(TA valueA, TB valueB, TC valueC, TD valueD) => _action.Invoke(valueA, valueB, valueC, valueD);

        public void Clear() => _action = Dummy;

        private static void Dummy(TA ta, TB tb, TC tc, TD td) { }
    }
}
