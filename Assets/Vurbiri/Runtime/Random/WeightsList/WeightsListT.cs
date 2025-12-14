using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public class WeightsList<T>
    {
        private const int BASE_CAPACITY = 5;
        private static readonly IEqualityComparer<T> s_comparer = EqualityComparer<T>.Default;

        private Weight[] _weights;
        private int _count, _capacity, _max;

        public T this[int index] { [Impl(256)] get { Throw.IfIndexOutOfRange(index, _count - 1); return _weights[index + 1].value; } }
        public int Count { [Impl(256)] get => _count - 1; }
        public T Roll { [Impl(256)] get => _weights[GetRandomIndex()].value; }

        [Impl(256)] public WeightsList() : this(default, BASE_CAPACITY) { }
        [Impl(256)] public WeightsList(int capacity) : this(default, capacity) { }
        [Impl(256)] public WeightsList(T zero) : this(zero, BASE_CAPACITY) { }
        public WeightsList(T zero, int capacity)
        {
            _capacity = capacity + 1;
            _weights = new Weight[_capacity];
            _weights[0] = new(zero, 0);
            _count = 1;
        }

        public void Add(T value, int weight)
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

        [Impl(256)] public T Extract()
        {
            int index = GetRandomIndex();
            T value = _weights[index].value;
            RemoveAtInternal(index);
            return value;
        }

        [Impl(256)] public bool TryExtract(out T value)
        {
            int index = GetRandomIndex();
            value = _weights[index].value;
            return RemoveAtInternal(index);
        }

        [Impl(256)] public bool RemoveAt(int index) => RemoveAtInternal(index + 1);
        [Impl(256)] public bool Remove(T item) => RemoveAtInternal(FindIndex(item));
        [Impl(256)] public int IndexOf(T item) => FindIndex(item) - 1;
        [Impl(256)] public bool Contains(T item) => FindIndex(item) > 0;

        [Impl(256)] public void Clear()
        {
            if (_count > 1)
            {
                System.Array.Clear(_weights, 1, _count);
                _count = 1;
            }
        }

        [Impl(256)] public void TrimExcess() => ReSize(_count);

        protected int FindIndex(T item)
        {
            int index = _count;
            while (index --> 1 && !s_comparer.Equals(item, _weights[index].value)) ;
            return index;
        }

        protected int GetRandomIndex()
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

        protected bool RemoveAtInternal(int index)
        {
            bool result = index > 0 & index < _count;
            if (result)
            {
                _count--;
                int delta = _weights[index] - _weights[index - 1];
                for (; index < _count; ++index)
                    _weights[index] = _weights[index + 1].Remove(delta);
                _weights[_count] = null;
                _max -= delta;
            }
            return result;
        }

        protected T Search(int min, int max, int weight)
        {
            int current = min + max >> 1;

            if (_weights[current] <= weight)
                return Search(current, max, weight);
            if (_weights[current - 1] > weight)
                return Search(min, current, weight);

            return _weights[current].value;
        }

        [Impl(256)] private void ReSize(int newCapacity)
        {
            _capacity = newCapacity;
            System.Array.Resize(ref _weights, newCapacity);
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

            sealed public override string ToString() => _weight.ToString();

            [Impl(256)] public static implicit operator int(Weight self) => self._weight;

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

        //public void Shuffle()
        //{
        //    int newIndex = 1, maxWeight = 0;
        //    var weights = new Weight[_capacity];
        //    Weight temp;

        //    weights[0] = _weights[0];
        //    foreach (int oldIndex in new RandomSequence(1, _count))
        //    {
        //        temp = _weights[oldIndex];
        //        maxWeight += temp - _weights[oldIndex - 1];
        //        weights[newIndex++] = new(temp.value, maxWeight);
        //    }
        //    _weights = weights;
        //}
    }
}
