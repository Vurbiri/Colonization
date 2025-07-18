using System;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
	public abstract class WaitButton : System.Collections.IEnumerator
    {
        protected readonly Subscription<Id<MBButtonId>> _onResult = new();
        protected bool _keepWaiting;
        protected Id<MBButtonId> _id;

        public object Current => null;
        public Id<MBButtonId> Id => _id;

        public Unsubscription AddListener(Action<Id<MBButtonId>> action) => _onResult.Add(action);

        public bool MoveNext() => _keepWaiting;
        public void Reset() 
        {
            _keepWaiting = false;
            MessageBox.Abort(this);
        }
    }

    public class WaitButtonSource : WaitButton
    {
        public bool IsWait => _keepWaiting;

        public WaitButtonSource()
        {
            _keepWaiting = true;
        }

        public void SetResult(Id<MBButtonId> result)
        {
            _id = result;
            _keepWaiting = false;

            _onResult.Invoke(result);
        }
    }
}
