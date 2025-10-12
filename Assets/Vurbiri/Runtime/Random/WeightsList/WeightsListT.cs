using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public class WeightsList<T>
    {
        private const int BASE_CAPACITY = 7;

        private Weight[] _weights;
        private int _count, _capacity, _max;

        public int Count { [Impl(256)] get => _count - 1; }

        public T Value { [Impl(256)] get => _weights[Index].value; }
        public int Index 
        {
            get 
            {
                int index = 0;
                if (_count > 1)
                {
                    int weight = UnityEngine.Random.Range(0, _max);
                    int min = 0, max = _count, current;
                    while (index == 0)
                    {
                        current = min + max >> 1;
                        if (_weights[current] <= weight)
                            min = current;
                        else if (_weights[current - 1] > weight)
                            max = current;
                        else
                            index = current;
                    }
                }
                return index;
            }
        }

        [Impl(256)] public WeightsList(T zero) : this(zero, BASE_CAPACITY) { }
        [Impl(256)] public WeightsList(T zero, int capacity)
        {
            _capacity = capacity + 1;
            _weights = new Weight[_capacity];
            _weights[0] = new(zero, 0);
            _count = 1;
        }

        [Impl(256)] public void Add(T value, int weight)
        {
            if (_count == _capacity)
                ReSize(_capacity << 1 | BASE_CAPACITY);

            _max = _weights[_count - 1] + weight;
            _weights[_count++] = new(value, _max);
        }

        [Impl(256)] public bool TryAdd(T value, int weight)
        {
            bool result = weight > 0;
            if (result)
                Add(value, weight);
            return result;
        }

        public bool RemoveAt(int index)
        {
            bool result = ++index > 0 & index < _count;
            if (result)
            {
                _count--;
                int delta = _weights[index] - _weights[index - 1];
                for (; index < _count; index++)
                    _weights[index] = _weights[index + 1].Remove(delta);
                _weights[_count] = null;
                _max -= delta;
            }
            return result;
        }

        [Impl(256)] public bool TryRemove<U>(U item) where U : System.IEquatable<T> => RemoveAt(IndexOf(item));
        [Impl(256)] public void Remove<U>(U item) where U : System.IEquatable<T> => RemoveAt(IndexOf(item));

        [Impl(256)] public int IndexOf<U>(U item) where U : System.IEquatable<T>
        {
            int index = _count;
            while (index --> 1 && !item.Equals(_weights[index].value));
            return index - 1;
        }

        public void Clear()
        {
            for(int i = 1; i < _count; i++)
                _weights[i] = null;
            _count = 1;
        }

        [Impl(256)] public void TrimExcess() => ReSize(_count);

        private void ReSize(int newCapacity)
        {
            _capacity = newCapacity;

            var array = new Weight[newCapacity];
            for (int i = 0; i < _count; i++)
                array[i] = _weights[i];
            _weights = array;
        }


        private T Search(int min, int max, int weight)
        {
            int current = min + max >> 1;

            if (_weights[current] <= weight)
                return Search(current, max, weight);
            if (_weights[current - 1] > weight)
                return Search(min, current, weight);

            return _weights[current].value;
        }

        #region Nested Weight
        //**********************************************************
        private class Weight
        {
            private int _weight;

            public readonly T value;

            [Impl(256)] public Weight(T obj, int weight)
            {
                value = obj;
                _weight = weight;
            }

            [Impl(256)] public Weight Add(int delta)
            {
                _weight += delta;
                return this;
            }

            [Impl(256)] public Weight Remove(int delta)
            {
                _weight -= delta;
                return this;
            }


            public static implicit operator int(Weight self) => self._weight;

            [Impl(256)] public static int operator +(Weight w1, Weight w2) => w1._weight + w2._weight;
            [Impl(256)] public static int operator +(Weight w, int i) => w._weight + i;
            [Impl(256)] public static int operator -(Weight w1, Weight w2) => w1._weight - w2._weight;
            [Impl(256)] public static int operator -(Weight w, int i) => w._weight - i;

            [Impl(256)] public static bool operator >(Weight w, int i) => w._weight > i;
            [Impl(256)] public static bool operator <(Weight w, int i) => w._weight < i;
            [Impl(256)] public static bool operator >=(Weight w, int i) => w._weight >= i;
            [Impl(256)] public static bool operator <=(Weight w, int i) => w._weight <= i;

            [Impl(256)] public static bool operator >(int i, Weight w) => i > w._weight;
            [Impl(256)] public static bool operator <(int i, Weight w) => i < w._weight;
            [Impl(256)] public static bool operator >=(int i, Weight w) => i >= w._weight;
            [Impl(256)] public static bool operator <=(int i, Weight w) => i <= w._weight;
        }
        #endregion
    }
}
