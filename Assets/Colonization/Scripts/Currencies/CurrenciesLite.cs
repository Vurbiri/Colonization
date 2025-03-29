//Assets\Colonization\Scripts\Currencies\CurrenciesLite.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Vurbiri.Colonization
{
    [Serializable]
    sealed public class CurrenciesLite : ACurrencies
    {
        [SerializeField] private int[] _values = new int[CurrencyId.CountAll];
        [SerializeField] private int _amount = 0;

        public override int Amount { get => _amount;}

        public override int this[int index] { get => _values[index]; }
        public override int this[Id<CurrencyId> id] { get => _values[id.Value]; }

        public CurrenciesLite() { }
        public CurrenciesLite(int[] array)
        {
            int value;
            for (int i = 0; i < countAll; i++)
            {
                value = array[i];
                _values[i] = value;
                _amount += value;
            }
        }

        public void Increment(int index)
        {
            _values[index]++;
            _amount++;
        }

        public void Set(int index, int value)
        {
            int temp = _values[index];
            _values[index] = value;
            _amount += value - temp;
        }

        public void Add(int index, int value)
        {
            _values[index] += value;
            _amount += value;
        }

        public void AddFrom(CurrenciesLite other)
        {
            if (other._amount == 0)
                return;

            for (int i = 0; i < countAll; i++)
                _values[i] += other._values[i];
            _amount += other._amount;
        }

        public void Multiply(int ratio)
        {
            if (_amount == 0)
                return;

            for (int i = 0; i < countMain; i++)
                _values[i] *= ratio;

            _amount *= ratio;
        }

        public void RandomMainAdd(int value)
        {
            _values[Random.Range(0, countMain)] += value;
            _amount += value;
        }

        public override IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < countAll; i++)
                yield return _values[i];
        }

        public static CurrenciesLite operator +(CurrenciesLite a, CurrenciesLite b)
        {
            if(a == null | b == null) return null;

            if (a._amount == 0) return b;
            if (b._amount == 0) return a;

            for (int i = 0; i < countAll; i++)
                a._values[i] += b._values[i];
            a._amount += b._amount;

            return a;
        }
    }
}
