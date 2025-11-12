using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public class WeightsList
	{
        private const int BASE_CAPACITY = 7;

        protected int[] _weights;
        protected int _count, _capacity;

        public int Count { [Impl(256)] get => _count - 1; }

        public int Roll
        {
            get
            {
                int index = 0;
                if (_count > 1)
                {
                    int weight = UnityEngine.Random.Range(0, _weights[_count - 1]);
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
                return index - 1;
            }
        }

        [Impl(256)] public WeightsList() : this(BASE_CAPACITY) { }
        [Impl(256)] public WeightsList(int capacity)
        {
            _capacity = capacity + 1;
            _weights = new int[_capacity];
            _weights[0] = 0;
            _count = 1;
        }

        [Impl(256)] public void Add(int weight)
        {
            if (_count == _capacity)
                ReSize(_capacity << 1 | BASE_CAPACITY);

            _weights[_count] = _weights[_count - 1] + weight;
            _count++;
        }

        [Impl(256)] public bool TryAdd(int weight)
        {
            bool result = weight > 0;
            if (result)
                Add(weight);
            return result;
        }

        public bool Remove(int index)
        {
            bool result = ++index > 0 & index < _count;
            if (result)
            {
                _count--;
                int delta = _weights[index] - _weights[index - 1];
                for (; index < _count; ++index)
                    _weights[index] = _weights[index + 1] - delta;
            }
            return result;
        }

        [Impl(256)] public void Clear() => _count = 1;

        [Impl(256)] public void TrimExcess() => ReSize(_count);

        private void ReSize(int newCapacity)
        {
            _capacity = newCapacity;

            var array = new int[newCapacity];
            for (int i = 0; i < _count; ++i)
                array[i] = _weights[i];
            _weights = array;
        }
    }
}
