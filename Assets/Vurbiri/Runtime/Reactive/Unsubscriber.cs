//Assets\Vurbiri\Runtime\Types\Reactive\Unsubscriber.cs
using System;

namespace Vurbiri.Reactive
{
    internal class Unsubscriber<TDelegate> : IUnsubscriber where TDelegate : Delegate
    {
        private ISubscriber<TDelegate> _subscriber;
        internal TDelegate action;

        internal Unsubscriber(ISubscriber<TDelegate> subscriber, TDelegate action)
        {
            _subscriber = subscriber;
            this.action = action;
        }

        public void Unsubscribe()
        {
            _subscriber?.Unsubscribe(this);
            _subscriber = null;
            action = null;
        }

        internal void Clear()
        {
            _subscriber = null;
            action = null;
        }
    }
}
