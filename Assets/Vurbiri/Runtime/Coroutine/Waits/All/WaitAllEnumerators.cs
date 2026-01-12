using System.Collections;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	sealed public class WaitAllEnumerators : AWait
	{
		private readonly List<Routine> _coroutines = new();

		public override bool IsWait { [Impl(256)] get => _coroutines.Count > 0; }
		public int Count { [Impl(256)] get => _coroutines.Count; }

		[Impl(256)] public WaitAllEnumerators() { }
		[Impl(256)] public WaitAllEnumerators(IEnumerator coroutine) => _coroutines.Add(new(coroutine));
		[Impl(256)] public WaitAllEnumerators(IEnumerator coroutine1, IEnumerator coroutine2) => Add(coroutine1, coroutine2);
		[Impl(256)] public WaitAllEnumerators(params IEnumerator[] coroutines) => Add(coroutines);
		[Impl(256)] public WaitAllEnumerators(IEnumerable<IEnumerator> enumerable) => AddRange(enumerable);

		[Impl(256)] public WaitAllEnumerators Add(IEnumerator coroutine)
		{
			_coroutines.Add(new(coroutine));
			return this;
		}
		[Impl(256)] public WaitAllEnumerators Add(IEnumerator coroutine1, IEnumerator coroutine2)
		{
			_coroutines.Add(new(coroutine1));
			_coroutines.Add(new(coroutine2));
			return this;
		}
		[Impl(256)] public WaitAllEnumerators Add(params IEnumerator[] coroutines)
		{
            for (int i = coroutines.Length - 1; i >= 0; --i)
                _coroutines.Add(new(coroutines[i]));
			return this;
		}
		[Impl(256)] public WaitAllEnumerators AddRange(IEnumerable<IEnumerator> enumerable)
		{
			foreach (IEnumerator coroutine in enumerable)
				_coroutines.Add(new(coroutine));
			return this;
		}

		public override bool MoveNext()
		{
            for (int i = _coroutines.Count - 1; i >= 0; --i)
                if (!_coroutines[i].MoveNext())
					_coroutines.RemoveAt(i);

			return _coroutines.Count > 0;
		}

        [Impl(256)] public void Clear() => _coroutines.Clear();

		// ************* Nested *************
		private class Routine
		{
			private readonly Stack<IEnumerator> _stack = new();
			private IEnumerator _current;

			[Impl(256)] public Routine(IEnumerator current) => _current = current;

			public bool MoveNext()
			{
				bool result = _current.MoveNext();
				if (result)
				{
					if (_current.Current is IEnumerator next)
					{
						_stack.Push(_current);
						_current = next;
					}
				}
				else if (_stack.Count > 0)
				{
					_current = _stack.Pop();
					result = true;
				}

				return result;
			}
		}
		// ************* Nested *************
	}
}
