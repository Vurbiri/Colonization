using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public class WaitAllWaits: AWait
	{
		private readonly List<AWait> _waits = new();

		public override bool IsWait { [Impl(256)] get => _waits.Count > 0; }
		public int Count { [Impl(256)] get => _waits.Count; }

		[Impl(256)] public WaitAllWaits() { }
		[Impl(256)] public WaitAllWaits(AWait wait) => _waits.Add(wait);
		[Impl(256)] public WaitAllWaits(AWait wait1, AWait wait2) => Add(wait1, wait2);
		[Impl(256)] public WaitAllWaits(params AWait[] waits) => Add(waits);
		[Impl(256)] public WaitAllWaits(IEnumerable<AWait> enumerable) => _waits.AddRange(enumerable);

		[Impl(256)] public WaitAllWaits Add(AWait wait)
		{
			_waits.Add(wait);
			return this;
		}
		[Impl(256)] public WaitAllWaits Add(AWait wait1, AWait wait2)
		{
			_waits.Add(wait1);
			_waits.Add(wait2);
			return this;
		}
		[Impl(256)] public WaitAllWaits Add(params AWait[] waits)
		{
			for (int i = waits.Length -1; i >=0 ; --i)
				_waits.Add(waits[i]);
			return this;
		}
		[Impl(256)] public WaitAllWaits AddRange(IEnumerable<AWait> enumerable)
		{
            _waits.AddRange(enumerable);
            return this;
		}

		public override bool MoveNext()
		{
			for (int i = _waits.Count - 1; i >= 0; --i)
				if (!_waits[i].MoveNext())
					_waits.RemoveAt(i);

			return _waits.Count > 0;
		}

        [Impl(256)] public void Clear() => _waits.Clear();
		
	}
}
