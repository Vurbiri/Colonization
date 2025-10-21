using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public class OutB : Out<bool> { }
    public class OutI : Out<int> { }
    public class OutF : Out<float> { }

    public class Out<T>
	{
		private readonly static Pool s_pool = new();

        private T _value;

        protected Out() { }

        [Impl(256)] public static Out<T> Get(out int key) => s_pool.Get(out key);
        [Impl(256)] public static Out<T> Get(T value, out int key)
        {
            var obj = s_pool.Get(out key); obj._value = value;
            return obj;
        }
        [Impl(256)] public static T Result(int key) => s_pool.Push(key);

        [Impl(256)] public void Set(T value) => _value = value;

        [Impl(256)] public static implicit operator T(Out<T> self) => self._value;

        // ************ Nested ******************
        private class Pool
        {
            private const int BASE_CAPACITY = 1;

            private Out<T>[] _values = new Out<T>[BASE_CAPACITY];
            private bool[] _keys = new bool[BASE_CAPACITY];
            private int _capacity, _count;

            public Pool()
            {
                for(int i = 0; i < BASE_CAPACITY; i++)
                {
                    _values[i] = new();
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
                        GrowArrays();

                    _values[key = _count++] = new();
                }
                _keys[key] = false;
                return _values[key];
            }

            [Impl(256)] public T Push(int key)
            {
                _keys[key] = true;
                return _values[key]._value;
            }

            [Impl(256)] private void GrowArrays()
            {
                _capacity = _capacity << 1 | BASE_CAPACITY;

                var values = new Out<T>[_capacity];
                for (int i = 0; i < _count; i++)
                    values[i] = _values[i];
                
                _values = values;
                _keys = new bool[_capacity];
            }
        }
    }
}
