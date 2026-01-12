using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	sealed public class WaitState<T> : AWait where T : Enum
	{
		private readonly WaitStateSource<T> _source;
		private readonly int _waitStateHashCode;

		public override bool IsWait { [Impl(256)] get => _waitStateHashCode != _source._stateHashCode; }

		internal WaitState(WaitStateSource<T> source, T waitValue)
		{
			_source = source;
			_waitStateHashCode = waitValue.GetHashCode();
		}

		public override bool MoveNext() => _waitStateHashCode != _source._stateHashCode;
	}

	public abstract class WaitStateSource<T> where T : Enum
	{
		internal int _stateHashCode;

		[Impl(256)] public WaitState<T> GetWaitState(T value) => new(this, value);
	}

	sealed public class WaitStateController<T> : WaitStateSource<T> where T : Enum
	{
		[Impl(256)] public WaitStateController() => _stateHashCode = default(T).GetHashCode();
		[Impl(256)] public WaitStateController(T defaultValue) => _stateHashCode = defaultValue.GetHashCode();

		[Impl(256)] public void SetState(T value) => _stateHashCode = value.GetHashCode();
		[Impl(256)] public void Reset() => _stateHashCode = default(T).GetHashCode();
	}
}
