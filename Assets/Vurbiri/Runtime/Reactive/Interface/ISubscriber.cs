//Assets\Vurbiri\Runtime\Reactive\Interface\ISubscriber.cs
using System;

namespace Vurbiri.Reactive
{
    public interface ISubscriber<TDelegate> where TDelegate : Delegate
    {
        public void Unsubscribe(TDelegate action);
    }
}
