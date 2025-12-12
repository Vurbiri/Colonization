using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
                _version.Next();
            }
        }

        public int Capacity { [Impl(256)] get => _capacity; }
        public bool IsReadOnly { [Impl(256)] get => false; }

        #region Constructors
        [Impl(256)] public Roster() : this(BASE_CAPACITY) { }
        [Impl(256)] public Roster(int capacity)
        {
            _capacity = capacity;
            _values = capacity == 0 ? s_empty : new TValue[capacity];
        }
        [Impl(256)] public Roster(params TValue[] values) : base(values) => _capacity = _count;
        [Impl(256), JsonConstructor] public Roster(ICollection<TValue> collection) : base(collection) => _capacity = _count;
        #endregion

        [Impl(256)] public void Add(TValue item)
        {
            if (_count == _capacity)
                GrowArray();

            _values[_count++] = item;
            _version.Next();
        }

        [Impl(256)] public bool TryAdd(TValue item)
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

            if (index < _count)
                Array.Copy(_values, index, _values, index + 1, _count - index);

            _values[index] = item;
            ++_count;
            _version.Next();
        }

        public bool Remove(TValue item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            Throw.IfIndexOutOfRange(index, _count);

            if (index < --_count)
                Array.Copy(_values, index + 1, _values, index, _count - index);

            _values[_count] = default;
            _version.Next();
        }

        public TValue Extract()
        {
            int index = UnityEngine.Random.Range(0, _count);
            TValue value = _values[index];
            RemoveAt(index);
            return value;
        }

        [Impl(256)] public void Clear()
        {
            if (_count > 0 && RuntimeHelpers.IsReferenceOrContainsReferences<TValue>())
                Array.Clear(_values, 0, _count);

            _count = 0;
            _version.Next();
        }

        [Impl(256)] public void CopyTo(TValue[] array, int arrayIndex) => Array.Copy(_values, 0, array, arrayIndex, _count);

        [Impl(256)] public void TrimExcess() 
        {
            if(_count != _capacity)
                GrowArray(_count);
        }

        [Impl(256)] private void GrowArray() => GrowArray(_capacity << 1 | BASE_CAPACITY);
        [Impl(256)] private void GrowArray(int capacity) => Array.Resize(ref _values, _capacity = capacity);
    }
}
