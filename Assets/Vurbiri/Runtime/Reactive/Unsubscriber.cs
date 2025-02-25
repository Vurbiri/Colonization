//Assets\Vurbiri\Runtime\Types\Reactive\Unsubscriber.cs
using System;

namespace Vurbiri.Reactive
{
    public class Unsubscriber<TDelegate> : IUnsubscriber where TDelegate : Delegate
    {
        private WeakReference<ISubscriber<TDelegate>> _weakSubscriber;
        private TDelegate action;

        public Unsubscriber(ISubscriber<TDelegate> subscriber, TDelegate action)
        {
            _weakSubscriber = new(subscriber);
            this.action = action;
        }

        public void Unsubscribe()
        {
            if(_weakSubscriber != null && _weakSubscriber.TryGetTarget(out ISubscriber<TDelegate> subscriber))
            {
                subscriber.Unsubscribe(action);

                _weakSubscriber.SetTarget(null);
                _weakSubscriber = null;
            }
            action = null;
        }
    }
}
