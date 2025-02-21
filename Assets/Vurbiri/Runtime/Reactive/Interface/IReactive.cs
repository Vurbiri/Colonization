//Assets\Vurbiri\Runtime\Types\Reactive\Interface\IReactive.cs
using System;

namespace Vurbiri.Reactive
{
    public interface IReactiveBase<in TAction> where TAction : Delegate
    {
        public IUnsubscriber Subscribe(TAction action, bool calling = true);
        public void Unsubscribe(TAction action);
    }

    public interface IReactive<out T> : IReactiveBase<Action<T>>
    {
    }

    public interface IReactive<out TA, out TB> : IReactiveBase<Action<TA, TB>>
    {
    }
}
