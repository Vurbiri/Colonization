using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Vurbiri.Colonization
{
    using static CurrencyId;

    [Serializable]
    sealed public class CurrenciesLite : ACurrencies
    {
        [SerializeField] private int[] _values = new int[CountAll];
        [SerializeField] private int _amount = 0;

        public override int Amount { get => _amount;}

        public override int this[int index] { get => _values[index]; }
        public override int this[Id<CurrencyId> id] { get => _values[id.Value]; }

        public CurrenciesLite() { }
        public CurrenciesLite(int[] array)
        {
            for (int i = 0; i < CountAll; i++)
            {
                _values[i] = array[i];
                _amount += array[i];
            }
        }
        public CurrenciesLite(CurrenciesLite other)
        {
            for (int i = 0; i < CountAll; i++)
                _values[i] = other._values[i];
            _amount = other._amount;
        }

        public void Increment(int index)
        {
            _values[index]++;
            _amount++;
        }

        public void Set(int index, int value)
        {
            _amount += value - _values[index];
            _values[index] = value;
        }

        public void Add(int index, int value)
        {
            _values[index] += value;
            _amount += value;
        }

        public void Add(CurrenciesLite other)
        {
            if (other._amount == 0)
                return;

            for (int i = 0; i < CountAll; i++)
                _values[i] += other._values[i];
            _amount += other._amount;
        }

        public void Multiply(int ratio)
        {
            if (_amount == 0)
                return;

            for (int i = 0; i < MainCount; i++)
                _values[i] *= ratio;

            _amount *= ratio;
        }

        public void RandomMainAdd(int value)
        {
            _values[Random.Range(0, MainCount)] += value;
            _amount += value;
        }

        public void Clear()
        {
            for (int i = 0; i < CountAll; i++)
                _values[i] = 0;

            _amount = 0;
        }

        public override IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < CountAll; i++)
                yield return _values[i];
        }

        public static CurrenciesLite operator +(CurrenciesLite a, CurrenciesLite b)
        {
            if(a == null | b == null) return null;

            if (a._amount == 0) return new(b);
            if (b._amount == 0) return new(a);

            CurrenciesLite sum = new();
            for (int i = 0; i < CountAll; i++)
                sum._values[i] = a._values[i] + b._values[i];
            sum._amount = a._amount + b._amount;

            return sum;
        }
        public static CurrenciesLite operator -(CurrenciesLite a, CurrenciesLite b)
        {
            if (a == null | b == null) return null;

            if (a._amount == 0) return -b;
            if (b._amount == 0) return new(a);

            CurrenciesLite diff = new();
            for (int i = 0; i < CountAll; i++)
                diff._values[i] = a._values[i] - b._values[i];
            diff._amount = a._amount - b._amount;

            return diff;
        }

        public static CurrenciesLite operator -(CurrenciesLite a)
        {
            if (a == null) return null;
            if (a._amount == 0) return new();

            CurrenciesLite neg = new();
            for (int i = 0; i < CountAll; i++)
                neg._values[i] = -a._values[i];
            neg._amount = -a._amount;

            return neg;
        }
    }
}
