using System;

namespace Vurbiri.Reactive
{
    public interface IReactiveBase<TAction> where TAction : Delegate
    {
        public IUnsubscriber Subscribe(TAction action, bool calling = true);
        public void Unsubscribe(TAction action);
    }

    public interface IReactive<T> : IReactiveBase<Action<T>>
    {
    }

    public interface IReactive<TA, TB> : IReactiveBase<Action<TA, TB>>
    {
    }
}
