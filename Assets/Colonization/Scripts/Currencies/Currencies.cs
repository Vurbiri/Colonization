using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Vurbiri.Colonization
{
    [JsonArray]
    public class Currencies : AReadOnlyCurrenciesReactive, IEnumerable<int>
    {
        public Currencies(IReadOnlyList<int> array) : base(array) { }
        public Currencies(ACurrencies other) : base(other) { }
        public Currencies() : base() { }

        public void Set(int index, int value) => Amount += _values[index].Set(value);
        public void Set(Id<CurrencyId> id, int value) => Amount += _values[id.ToInt].Set(value);

        public void Increment(int index) => Amount += _values[index].Increment();
        public void Increment(Id<CurrencyId> id) => Amount += _values[id.ToInt].Increment();

        public void Add(int index, int value)
        {
            if (value != 0)
                Amount += _values[index].Add(value);
        }
        public void Add(Id<CurrencyId> id, int value) => Add(id.ToInt, value);
        
        public void AddAndClampToBlood(int value, int max) => Amount += _values[CurrencyId.Blood].AddAndClamp(value, max);

        public void AddFrom(ACurrencies other)
        {
            if (other.Amount == 0)
                return;
            
            for (int i = 0; i < _countAll; i++)
                _amount += _values[i].Add(other[i]);

            ActionAmountChange?.Invoke(_amount);
        }

        public void Pay(ACurrencies cost)
        {
            for (int i = 0; i < _countAll; i++)
                _amount += _values[i].Add(-cost[i]);

            ActionAmountChange?.Invoke(_amount);
        }

        public void ClampMain(int max)
        {
            if (_amount <= max)
                return;

            int index = Random.Range(0, _countMain);
            while (_amount > max)
            {
                _amount += _values[index].DecrementNotMessage();
                index = ++index % _countMain;
            }

            for (int i = 0; i < _countMain; i++)
                _values[i].SendMessage();

            ActionAmountChange?.Invoke(_amount);
        }

        public void Clear()
        {
            for (int i = 0; i < _countAll; i++)
                _values[i].Set(0);
            Amount = 0;
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < _countAll; i++)
                yield return _values[i].Value;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
