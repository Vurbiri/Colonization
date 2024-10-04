using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;
using Random = UnityEngine.Random;

namespace Vurbiri.Colonization
{
    [Serializable, JsonArray]
    public class Currencies : AReactive<Currencies>, IEnumerable<int>
#if UNITY_EDITOR
        , ISerializationCallbackReceiver
#endif 
    {
        [SerializeField] private int[] _values;
        [SerializeField] private int _amount;
        private int _count;

        public int Count => _count;
        public int Amount => _amount;
        public IReadOnlyList<int> Values => _values;

        public int this[Id<CurrencyId> id] { get => _values[id.ToInt]; set => Set(id, value); }
        public int this[int index] { get => _values[index]; set => Set(index, value); }

        public Currencies(IReadOnlyList<int> array) : this()
        {
            int value, count = _count < array.Count ? _count : array.Count;

            for (int i = 0; i < count; i++)
            {
                value = array[i];
                _values[i] = value;
                _amount += value;
            }
        }
        public Currencies(Currencies other) : this()
        {
            for (int i = 0; i < _count; i++)
                _values[i] = other._values[i];

            _amount = other._amount;
        }
        public Currencies()
        {
            _count = CurrencyId.Count;
            _values = new int[_count];
            _amount = 0;
        }

        //TEST
        public void Rand(int max)
        {
            Debug.Log("TEST (Currencies)");
            for (int i = 0; i < _count; i++)
                Set(i, UnityEngine.Random.Range(0, max + 1));
        }

        public void Set(int index, int value)
        {
            _amount -= _values[index];
            _amount += value;

            _values[index] = value;

            ActionThisChange?.Invoke(this);
        }
        public void Set(Id<CurrencyId> id, int value) => Set(id.ToInt, value);

        public void SetFrom(IReadOnlyList<int> array)
        {
            int value, count = _count < array.Count ? _count : array.Count;

            for (int i = 0; i < count; i++)
            {
                value = array[i];
                _values[i] = value;
                _amount += value;
            }

            ActionThisChange?.Invoke(this);
        }
        public void SetFrom(Currencies other)
        {
            for (int i = 0; i < _count; i++)
                _values[i] = other._values[i];

            _amount = other._amount;

            ActionThisChange?.Invoke(this);
        }

        public void Increment(int index)
        {
            ++_values[index];
            ++_amount;

            ActionThisChange?.Invoke(this);
        }
        public void Increment(Id<CurrencyId> id) => Increment(id.ToInt);

        public void Add(int index, int value)
        {
            if (value == 0)
                return;

            _values[index] += value;
            _amount += value;

            ActionThisChange?.Invoke(this);
        }
        public void Add(Id<CurrencyId> id, int value) => Add(id.ToInt, value);

        public void RandomAdd(int value)
        {
            if (value == 0)
                return;

            _values[Random.Range(0, _count)] += value;
            _amount += value;

            ActionThisChange?.Invoke(this);
        }

        public void AddFrom(Currencies other)
        {
            if (other._amount == 0)
                return;
            
            for (int i = 0; i < _count; i++)
                _values[i] += other._values[i];

            _amount += other._amount;

            ActionThisChange?.Invoke(this);
        }

        public void Pay(Currencies cost)
        {
            for (int i = 0; i < _count; i++)
                _values[i] -= cost._values[i];

            _amount -= cost._amount;

            ActionThisChange?.Invoke(this);
        }

        public void Clamp(int max)
        {
            if (_amount <= max)
                return;

            int index;
            while(_amount > max)
            {
                index = Random.Range(0, _count);
                if (_values[index] > 0)
                {
                    _values[index]--;
                    _amount--;

                    //ActionThisChange?.Invoke(this);
                }
            }

            ActionThisChange?.Invoke(this);
        }

        public void Clear()
        {
            for (int i = 0; i < _count; i++)
                _values[i] = 0;
            _amount = 0;

            ActionThisChange?.Invoke(this);
        }

        protected override void Callback(Action<Currencies> action) => action(this);

        public static bool operator >(Currencies left, Currencies right) => !(left <= right);
        public static bool operator <(Currencies left, Currencies right) => !(left >= right);
        public static bool operator >=(Currencies left, Currencies right)
        {
            if (left == null || right == null || left._amount < right._amount)
                return false;

            for (int i = 0; i < left._count; i++)
                if (left._values[i] < right._values[i])
                    return false;
            return true;
        }
        public static bool operator <=(Currencies left, Currencies right)
        {
            if (left == null || right == null)
                return false;

            for (int i = 0; i < left._count; i++)
                if (left._values[i] > right._values[i])
                    return false;
            return true;
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
                yield return _values[i];
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #region ISerializationCallbackReceiver
#if UNITY_EDITOR
        public void OnBeforeSerialize()
        {
            if (Application.isPlaying)
                return;

            _count = CurrencyId.Count;
            if (_values.Length != _count)
                Array.Resize(ref _values, _count);

            _amount = 0;
            for (int i = 0; i < _count; i++)
                _amount += _values[i];
        }

        public void OnAfterDeserialize() { }
#endif
        #endregion
    }
}
