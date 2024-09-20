using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable, JsonArray]
    public class Currencies : EnumArray<CurrencyType, int>, IReactiveValue<Currencies>
    {
        [SerializeField] protected int _amount;
        public int Amount => _amount;

        public override int this[CurrencyType type] { get => _values[(int)type]; set => Add(type, value); }
        public override int this[int index] { get => _values[index]; set => Add(index, value); }

        private Action<Currencies> EventCurrenciesChange;
        event Action<Currencies> IReactiveValue<Currencies>.EventValueChange
        {
            add { EventCurrenciesChange += value; }
            remove { EventCurrenciesChange -= value; }
        }

        public Currencies(int[] array) : this()
        {
            int value, count = _count < array.Length ? _count : array.Length;

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
        public Currencies() : base() => _amount = 0;
        
        public void CopyFrom(Currencies other)
        {
            for (int i = 0; i < _count; i++)
                _values[i] = other._values[i];

            _amount = other._amount;

            EventCurrenciesChange?.Invoke(this);
        }

        public void AddFrom(Currencies other)
        {
            for (int i = 0; i < _count; i++)
                _values[i] += other._values[i];

            _amount += other._amount;

            EventCurrenciesChange?.Invoke(this);
        }

        //TEST
        public void Rand(int max)
        {
            Debug.Log("TEST");
            Clear();
            for (int i = 0; i < _count; i++)
                Add(i, UnityEngine.Random.Range(0, max + 1));

        }

        public void Clear()
        {
            for (int i = 0; i < _count; i++)
                _values[i] = 0;
            _amount = 0;

            EventCurrenciesChange?.Invoke(this);
        }

        public void Add(int id, int value)
        {
            _values[id] += value;
            _amount += value;

            EventCurrenciesChange?.Invoke(this);
        }
        public void Add(CurrencyType type, int value) => Add(id: (int)type, value);

        public void Pay(Currencies cost)
        {
            for (int i = 0; i < _count; i++)
                _values[i] -= cost._values[i];

            _amount -= cost._amount;

            EventCurrenciesChange?.Invoke(this);
        }

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

        public override void OnBeforeSerialize()
        {
            base.OnBeforeSerialize();

            _amount = 0;
            for (int i = 0; i < _count; i++)
                _amount += _values[i];
        }
    }
}
