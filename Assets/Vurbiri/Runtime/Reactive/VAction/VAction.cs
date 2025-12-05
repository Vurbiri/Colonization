using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public abstract class Event : IUnsubscribed<Action>
    {
        protected internal Action _action;

        public event Action Action { [Impl(256)] add { _action += value; } [Impl(256)] remove { _action -= value; } }

        [Impl(256)] public Subscription Add(Action action)
        {
            _action += action;
            return Subscription.Create(this, action);
        }

        [Impl(256)] public void Remove(Action action) => _action -= action;
    }
    //----------------------------------------------
    public class VAction : Event
    {
        [Impl(256)] public VAction() => _action = Dummy.Action;

        [Impl(256)] public void Invoke() => _action();
        [Impl(256)] public void InvokeOneShot() { _action(); _action = Dummy.Action; }

        [Impl(256)] public void Clear() => _action = Dummy.Action;
    }
    //=======================================================================================
    public abstract class Event<T> : IUnsubscribed<Action<T>>
    {
        protected internal Action<T> _action;

        public event Action<T> Action { [Impl(256)] add { _action += value; } [Impl(256)] remove { _action -= value; } }

        [Impl(256)] public Subscription Add(Action<T> action)
        {
            _action += action;
            return Subscription.Create(this, action);
        }

        [Impl(256)] public void Remove(Action<T> action) => _action -= action;
    }
    //----------------------------------------------
    public class VAction<T> : Event<T>
    {
        [Impl(256)] public VAction() => _action = Dummy.Action;

        [Impl(256)] public void Invoke(T value) => _action.Invoke(value);
        [Impl(256)] public void InvokeOneShot(T value) { _action(value); _action = Dummy.Action; }

        [Impl(256)] public Subscription Add(Action<T> action, T value)
        {
            action(value);

            _action += action;
            return Subscription.Create(this, action);
        }
        [Impl(256)] public Subscription Add(Action<T> action, T value, bool instantGetValue)
        {
            if (instantGetValue) action(value);

            _action += action;
            return Subscription.Create(this, action);
        }

        [Impl(256)] public void Clear() => _action = Dummy.Action;

        [Impl(256)] public static ComboAction<T> operator +(VAction action, VAction<T> actionT) => new(action, actionT);
        [Impl(256)] public static ComboAction<T> operator +(VAction<T> actionT, VAction action) => new(action, actionT);
    }
    //=======================================================================================
    public abstract class Event<TA, TB> : IUnsubscribed<Action<TA, TB>>
    {
        protected Action<TA, TB> _action;

        public event Action<TA, TB> Action { [Impl(256)] add { _action += value; } [Impl(256)] remove { _action -= value; } }

        [Impl(256)] public Subscription Add(Action<TA, TB> action)
        {
            _action += action;
            return Subscription.Create(this, action);
        }

        [Impl(256)] public void Remove(Action<TA, TB> action) => _action -= action;
    }
    //----------------------------------------------
    public class VAction<TA, TB> : Event<TA, TB>
    {
        [Impl(256)] public VAction() => _action = Dummy.Action;

        [Impl(256)] public void Invoke(TA valueA, TB valueB) => _action.Invoke(valueA, valueB);
        [Impl(256)] public void InvokeOneShot(TA valueA, TB valueB) { _action(valueA, valueB); _action = Dummy.Action; }
                
        [Impl(256)] public Subscription Add(Action<TA, TB> action, TA valueA, TB valueB)
        {
            action(valueA, valueB);

            _action += action;
            return Subscription.Create(this, action);
        }
        [Impl(256)] public Subscription Add(Action<TA, TB> action, TA valueA, TB valueB, bool instantGetValue)
        {
            if (instantGetValue) action(valueA, valueB);

            _action += action;
            return Subscription.Create(this, action);
        }

        [Impl(256)] public void Clear() => _action = Dummy.Action;
    }
    //=======================================================================================
    public abstract class Event<TA, TB, TC> : IUnsubscribed<Action<TA, TB, TC>>
    {
        protected Action<TA, TB, TC> _action;

        public event Action<TA, TB, TC> Action { [Impl(256)] add { _action += value; } [Impl(256)] remove { _action -= value; } }
        
        [Impl(256)] public Subscription Add(Action<TA, TB, TC> action)
        {
            _action += action;
            return Subscription.Create(this, action);
        }

        [Impl(256)] public void Remove(Action<TA, TB, TC> action) => _action -= action;
    }
    //----------------------------------------------
    public class VAction<TA, TB, TC> : Event<TA, TB, TC>
    {
        [Impl(256)] public VAction() => _action = Dummy.Action;

        [Impl(256)] public void Invoke(TA valueA, TB valueB, TC valueC) => _action.Invoke(valueA, valueB, valueC);
        [Impl(256)] public void InvokeOneShot(TA valueA, TB valueB, TC valueC) { _action(valueA, valueB, valueC); _action = Dummy.Action; }
                
        [Impl(256)] public Subscription Add(Action<TA, TB, TC> action, TA valueA, TB valueB, TC valueC)
        {
            action(valueA, valueB, valueC);

            _action += action;
            return Subscription.Create(this, action);
        }
        [Impl(256)] public Subscription Add(Action<TA, TB, TC> action, TA valueA, TB valueB, TC valueC, bool instantGetValue)
        {
            if (instantGetValue) action(valueA, valueB, valueC);

            _action += action;
            return Subscription.Create(this, action);
        }

        [Impl(256)] public void Clear() => _action = Dummy.Action;
    }
    //=======================================================================================
    public abstract class Event<TA, TB, TC, TD> : IUnsubscribed<Action<TA, TB, TC, TD>>
    {
        protected Action<TA, TB, TC, TD> _action;

        public event Action<TA, TB, TC, TD> Action { [Impl(256)] add { _action += value; } [Impl(256)] remove { _action -= value; } }

        [Impl(256)] public Subscription Add(Action<TA, TB, TC, TD> action)
        {
            _action += action;
            return Subscription.Create(this, action);
        }

        [Impl(256)] public void Remove(Action<TA, TB, TC, TD> action) => _action -= action;
    }
    //----------------------------------------------
    public class VAction<TA, TB, TC, TD> : Event<TA, TB, TC, TD>
    {
        [Impl(256)] public VAction() => _action = Dummy.Action;

        [Impl(256)] public void Invoke(TA valueA, TB valueB, TC valueC, TD valueD) => _action.Invoke(valueA, valueB, valueC, valueD);
        [Impl(256)] public void InvokeOneShot(TA valueA, TB valueB, TC valueC, TD valueD) { _action(valueA, valueB, valueC, valueD); _action = Dummy.Action; }
                
        [Impl(256)] public Subscription Add(Action<TA, TB, TC, TD> action, TA valueA, TB valueB, TC valueC, TD valueD)
        {
            action(valueA, valueB, valueC, valueD);

            _action += action;
            return Subscription.Create(this, action);
        }
        [Impl(256)] public Subscription Add(Action<TA, TB, TC, TD> action, TA valueA, TB valueB, TC valueC, TD valueD, bool instantGetValue)
        {
            if (instantGetValue) action(valueA, valueB, valueC, valueD);

            _action += action;
            return Subscription.Create(this, action);
        }

        [Impl(256)] public void Clear() => _action = Dummy.Action;
    }
}
