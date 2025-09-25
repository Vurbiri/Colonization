using System;

namespace Vurbiri.Reactive
{
    public interface IReactiveBase<in TDelegate> where TDelegate : Delegate
    {
        public Subscription Subscribe(TDelegate action, bool instantGetValue = true);
    }

    public interface IReactive<T> : IReactiveBase<Action<T>> {}
    public interface IReactive<TA, TB> : IReactiveBase<Action<TA, TB>> { }
    public interface IReactive<TA, TB, TC> : IReactiveBase<Action<TA, TB, TC>> { }
    public interface IReactive<TA, TB, TC, TD> : IReactiveBase<Action<TA, TB, TC, TD>> { }
}
