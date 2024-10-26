using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class Currencies : AReadOnlyCurrenciesReactive
    {
        public Currencies(IReadOnlyList<int> array) : base(array) { }
        public Currencies(ACurrencies other) : base(other) { }
        public Currencies() : base() { }

        public void Set(int index, int value) => Amount += _values[index].Set(value);
        public void Set(Id<CurrencyId> id, int value) => Amount += _values[id.Value].Set(value);

        public void Increment(int index) => Amount += _values[index].Increment();
        public void Increment(Id<CurrencyId> id) => Amount += _values[id.Value].Increment();

        public void Add(int index, int value)
        {
            if (value != 0)
                Amount += _values[index].Add(value);
        }
        public void Add(Id<CurrencyId> id, int value) => Add(id.Value, value);
        
        public void AddAndClampBlood(int value, int max) => Amount += _values[CurrencyId.Blood].AddAndClamp(value, max);

        public void AddFrom(ACurrencies other)
        {
            if (other.Amount == 0)
                return;
            
            for (int i = 0; i < countAll; i++)
                _amount += _values[i].Add(other[i]);

            ActionAmountChange?.Invoke(_amount);
        }

        public void Pay(ACurrencies cost)
        {
            for (int i = 0; i < countAll; i++)
                _amount += _values[i].Add(-cost[i]);

            ActionAmountChange?.Invoke(_amount);
        }

        public void ClampMain(int max)
        {
            if (_amount <= max)
                return;

            int index = Random.Range(0, countMain);
            while (_amount > max)
            {
                _amount += _values[index].DecrementNotMessage();
                index = ++index % countMain;
            }

            for (int i = 0; i < countMain; i++)
                _values[i].SendMessage();

            ActionAmountChange?.Invoke(_amount);
        }

        public void Clear()
        {
            for (int i = 0; i < countAll; i++)
                _values[i].Set(0);
            Amount = 0;
        }

        public int[] ToArray()
        {
            int[] result = new int[countAll];
            for (int i = 0; i < countAll; i++)
                result[i] = _values[i].Value;

            return result;
        }
    }
}
