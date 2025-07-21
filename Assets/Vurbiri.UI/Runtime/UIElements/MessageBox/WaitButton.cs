using System;

namespace Vurbiri.UI
{
	public abstract class WaitButton : IWait
    {
        protected Action<Id<MBButtonId>> _onResult;
        protected bool _isWait;
        protected Id<MBButtonId> _id;

        public object Current => null;
        public Id<MBButtonId> Id => _id;
        public bool IsWait => _isWait;

        public void AddListener(Action<Id<MBButtonId>> action) => _onResult += action;

        public bool MoveNext() => _isWait;
        public void Reset() 
        {
            if (_isWait)
            {
                _isWait = false;
                MessageBox.Abort(this);
            }
        }
    }

    public class WaitButtonSource : WaitButton
    {
        public WaitButtonSource()
        {
            _isWait = true;
        }

        public void SetResult(Id<MBButtonId> result)
        {
            _id = result;
            _isWait = false;
            _onResult?.Invoke(result);
        }
    }
}
