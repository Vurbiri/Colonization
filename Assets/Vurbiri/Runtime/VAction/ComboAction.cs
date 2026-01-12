using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public abstract class ComboEvent<T> : IUnsubscribed<Action>, IUnsubscribed<Action<T>>
    {
        protected Action _action;
        protected Action<T> _actionT;

        [Impl(256)] public Subscription Add(Action action)
        {
            _action += action;
            return Subscription.Create(this, action);
        }
        
        [Impl(256)] public Subscription Add(Action<T> action)
        {
            _actionT += action;
            return Subscription.Create(this, action);
        }

        [Impl(256)] public Subscription Add(Action action, Action<T> actionT)
        {
            _action += action; _actionT += actionT;
            return Subscription.Create(this, action) + Subscription.Create(this, actionT);
        }

        [Impl(256)] public void Remove(Action action) => _action -= action;
        [Impl(256)] public void Remove(Action<T> action) => _actionT -= action;
        [Impl(256)] public void Remove(Action action, Action<T> actionT)
        {
            _action -= action; _actionT -= actionT;
        }
    }
    //----------------------------------------------
    public class ComboAction<T> : ComboEvent<T>
    {
        [Impl(256)] public ComboAction()
        {
            _action = Dummy.Action; _actionT = Dummy.Action;
        }
        [Impl(256)] public ComboAction(VAction action, VAction<T> actionT)
        {
            _action = action._action; _actionT = actionT._action;
        }

        [Impl(256)] public void Invoke(T value)
        {
            _action.Invoke(); _actionT.Invoke(value);
        }
        [Impl(256)] public void InvokeOneShot(T value)
        {
            Invoke(value); Clear();
        }

        [Impl(256)] public void Clear()
        {
            _action = Dummy.Action; _actionT = Dummy.Action;
        }

        [Impl(256)] public Subscription Add(Action<T> action, T value)
        {
            action(value);

            _actionT += action;
            return Subscription.Create(this, action);
        }
        [Impl(256)] public Subscription Add(Action<T> action, T value, bool instantGetValue)
        {
            if (instantGetValue) action(value);

            _actionT += action;
            return Subscription.Create(this, action);
        }
    }
}
