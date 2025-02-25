//Assets\Vurbiri\Runtime\Types\Reactive\Interface\IReactive.cs
using System;

namespace Vurbiri.Reactive
{
    public interface IReactiveBase<in TAction> where TAction : Delegate
    {
        public IUnsubscriber Subscribe(TAction action, bool calling = true);
    }

    public interface IReactiveBase<TId, in TAction> where TAction : Delegate
    {
        public IUnsubscriber Subscribe(TId id, TAction action, bool calling = true);
    }

    public interface IReactive<out T> : IReactiveBase<Action<T>> {}

    public interface IReactive<out TA, out TB> : IReactiveBase<Action<TA, TB>> { }
    
    public interface IReactive<out TA, out TB, out TC> : IReactiveBase<Action<TA, TB, TC>> { }

    public interface IReactive<out TA, out TB, out TC, out TD> : IReactiveBase<Action<TA, TB, TC, TD>> { }
}
