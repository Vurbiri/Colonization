//Assets\Vurbiri\Runtime\Reactive\Interface\ISubscriber.cs
using System;

namespace Vurbiri.Reactive
{
    internal interface ISubscriber<TDelegate> where TDelegate : Delegate
    {
        internal void Unsubscribe(Unsubscriber<TDelegate> unsubscriber);
    }
}
