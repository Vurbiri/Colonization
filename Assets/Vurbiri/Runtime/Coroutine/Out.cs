using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public class Out<T>
	{
		private readonly static Pool s_pool = new();

		private bool _set;
		private T _value;

		protected Out() { }

		[Impl(256)] public static Out<T> Get(out int key) => s_pool.Get(out key);

		[Impl(256)] public static T Result(int key) => s_pool.Push(key);

		[Impl(256)] public void Set(T value)
		{
			_set = true; _value = value;
		}
		[Impl(256)] private Out<T> Get()
		{
			_set = false; _value = default;
			return this;
		}

		[Impl(256)] public static implicit operator T(Out<T> self) => self._value;


		// ************ Nested ******************
		private class Pool
		{
			private const int BASE_CAPACITY = 1;

			private Out<T>[] _outs = new Out<T>[BASE_CAPACITY];
			private bool[] _keys = new bool[BASE_CAPACITY];
			private int _capacity, _count;

			public Pool()
			{
				for(int i = 0; i < BASE_CAPACITY; ++i)
				{
					_outs[i] = new();
					_keys[i] = true;
				}
				_capacity = _count = BASE_CAPACITY;
			}

			public Out<T> Get(out int key)
			{
				key = _count;
				while (key --> 0 && !_keys[key]);

				if (key < 0)
				{
					if (_count == _capacity)
					{
						System.Array.Resize(ref _outs, _capacity = _capacity << 1 | BASE_CAPACITY);
						_keys = new bool[_capacity];
					}

					_outs[key = _count++] = new();
				}

				_keys[key] = false;
				return _outs[key].Get();
			}

			public T Push(int key)
			{
				var outT = _outs[key];
				if (outT._set & !_keys[key])
				{
					_keys[key] = true;
					return outT._value;
				}

				throw new("Value is not set.");
			}
		}
	}
}
