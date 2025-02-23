//Assets\Vurbiri\Runtime\Types\Reactive\Unsubscriber.cs
using System;

namespace Vurbiri.Reactive
{
    public class Unsubscriber<TDelegate> : IUnsubscriber where TDelegate : Delegate
    {
        private IReactiveBase<TDelegate> _reactive;
        private TDelegate _listener;

        public Unsubscriber(IReactiveBase<TDelegate> reactive, TDelegate listener)
        {
            _reactive = reactive;
            _listener = listener;
        }

        public void Unsubscribe()
        {
            _reactive?.Unsubscribe(_listener);
            _reactive = null;
            _listener = null;
        }
    }

    public class Unsubscriber<TId, TDelegate> : IUnsubscriber where TDelegate : Delegate
    {
        private IReactiveBase<TId, TDelegate> _reactive;
        private TDelegate _listener;
        private TId _id;

        public Unsubscriber(IReactiveBase<TId, TDelegate> reactive, TId id, TDelegate listener)
        {
            _reactive = reactive;
            _id = id;
            _listener = listener;
        }

        public void Unsubscribe()
        {
            _reactive?.Unsubscribe(_id, _listener);
            _reactive = null;
            _listener = null;
        }
    }
}
