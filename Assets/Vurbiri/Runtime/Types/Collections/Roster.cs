using Newtonsoft.Json;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Collections
{
    [JsonArray]
    public class Roster<TValue> : ReadOnlyArray<TValue>, IList<TValue>
    {
        private const int BASE_CAPACITY = 3;
        private int _capacity;

        public new TValue this[int index]
        {
            [Impl(256)] get
            {
                Throw.IfIndexOutOfRange(index, _count);
                return _values[index];
            }
            [Impl(256)] set
            {
                Throw.IfIndexOutOfRange(index, _count);
                _values[index] = value;
            }
        }

        public int Capacity { [Impl(256)] get => _capacity; }
        public bool IsReadOnly { [Impl(256)] get => false; }

        #region Constructors
        [Impl(256)] public Roster() : this(BASE_CAPACITY) { }
        [Impl(256)] public Roster(int capacity)
        {
            _capacity = capacity;
            _values = new TValue[_capacity];
        }
        [Impl(256)] public Roster(params TValue[] values) : base(values) => _capacity = _count;
        [Impl(256)] public Roster(List<TValue> values) : base(values) => _capacity = _count;
        [Impl(256), JsonConstructor] public Roster(IReadOnlyList<TValue> values) : base(values) => _capacity = _count;
        #endregion

        public void Add(TValue item)
        {
            if (_count == _capacity)
                GrowArray();

            _values[_count++] = item;
        }

        public bool TryAdd(TValue item)
        {
            bool result = !Contains(item);
            if (result) Add(item); 
            return result;
        }

        public void Insert(int index, TValue item)
        {
            Throw.IfIndexOutOfRange(index, _count);

            if (_count == _capacity)
                GrowArray();

            for (int i = _count; i > index; i--)
                _values[i] = _values[i - 1];

            _values[index] = item;
            _count++;
        }

        public bool Remove(TValue item)
        {
            int index = -1;
            while (++index < _count && !_values[index].Equals(item));
            if (index < _count)
                RemoveAt(index);

            return index < _count;
        }

        public void RemoveAt(int index)
        {
            Throw.IfIndexOutOfRange(index, _count);

            _count--;
            for (int i = index; i < _count; i++)
                _values[i] = _values[i + 1];

            _values[_count] = default;
        }

        public void Clear()
        {
            for (int i = 0; i < _count; i++)
                _values[i] = default;

            _count = 0;
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < _count; i++)
                array[i] = _values[i];
        }

        [Impl(256)] public void TrimExcess() 
        {
            if(_count != _capacity)
                GrowArray(_count);
        }

        [Impl(256)] private void GrowArray() => GrowArray(_capacity << 1 | BASE_CAPACITY);
        private void GrowArray(int capacity)
        {
            var array = new TValue[capacity];
            for (int i = 0; i < _count; i++)
                array[i] = _values[i];

            _values = array;
            _capacity = capacity;
        }
    }
}
