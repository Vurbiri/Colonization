using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	sealed public class WaitFrames : AWait
	{
		private ushort _waitFrames;
		private ushort _currentFrames;
		private bool _isWait;

		public override bool IsWait { [Impl(256)] get => _isWait; }

		public override bool MoveNext()
		{
			if (!_isWait)
				_currentFrames = _waitFrames;

			return _isWait = _currentFrames --> 0;
		}

		[Impl(256)] public WaitFrames(ushort frames)  => _waitFrames = frames;

		[Impl(256)] public WaitFrames Restart()
		{
			_isWait = false;
			return this;
		}

		[Impl(256)] public WaitFrames Restart(ushort value)
		{
			_waitFrames = value;
			_isWait = false;
			return this;
		}
	}
}
