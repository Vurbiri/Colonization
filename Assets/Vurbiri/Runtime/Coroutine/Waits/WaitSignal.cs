using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	sealed public class WaitSignal : AWait
	{
		private bool _isWait = true;

		public override bool IsWait { [Impl(256)] get => _isWait; }

		public override bool MoveNext() => _isWait;
		[Impl(256)] public void Send() => _isWait = false;
		[Impl(256)] public void Cancel() => _isWait = false;
		[Impl(256)] public override void Reset() => _isWait = true;
		[Impl(256)] public WaitSignal Restart() {_isWait = true; return this; }
	}
}
