using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
	public abstract class WaitButton : IWait
	{
		protected bool _isWait = true;
		protected Id<MBButtonId> _id = MBButtonId.None;
        protected Action<Id<MBButtonId>> _onResult = Dummy.Action;

        public object Current => null;
		public Id<MBButtonId> Id { [Impl(256)] get => _id; }
		public bool IsWait { [Impl(256)] get => _isWait; }

		[Impl(256)] public void AddListener(Action<Id<MBButtonId>> action) => _onResult += action;

		[Impl(256)] public void Reset() 
		{
			if (_isWait)
			{
				_isWait = false;
				MessageBox.Abort(this);
			}
		}

		public bool MoveNext() => _isWait;
	}

	public class WaitButtonSource : WaitButton
	{
        public void Set(Id<MBButtonId> result)
		{
            _isWait = false;
            _id = result;
			_onResult(result);
        }
    }
}
