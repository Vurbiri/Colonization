using System.Collections.Generic;
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
        [Impl(256)] public static Out<T> operator +(Out<T> self, T value)
        {
            self._value = value;
            return self;
        }

        // ************ Nested ******************
        private class Pool
        {
            private const int BASE_CAPACITY = 1;

            private readonly Stack<int> _keys = new(BASE_CAPACITY);
            private Out<T>[] _values = new Out<T>[BASE_CAPACITY];
            private int _capacity, _count;

            public Pool()
            {
                for(int i = 0; i < BASE_CAPACITY; i++)
                {
                    _keys.Push(i);
                    _values[i] = new();
                }
                _capacity = _count = BASE_CAPACITY;
            }

            public Out<T> Get(out int key)
            {
                if (_keys.Count > 0)
                {
                    key = _keys.Pop();
                }
                else
                {
                    if (_count == _capacity)
                    {
                        _capacity = _capacity << 1 | BASE_CAPACITY;

                        var array = new Out<T>[_capacity];
                        for (int i = 0; i < _count; i++)
                            array[i] = _values[i];
                        _values = array;
                    }

                    _values[key = _count++] = new();
                }
                return _values[key];
            }

            [Impl(256)] public T Push(int key)
            {
                _keys.Push(key);
                return _values[key]._value;
            }
        }
    }
}
