namespace Vurbiri.Colonization
{
    using System.Collections.Generic;
    using Vurbiri.Reactive;

    public class Currencies : AReadOnlyCurrenciesReactive
    {
        #region Constructions
        public Currencies(IReadOnlyList<int> array, IReactive<int> maxValueMain, IReactive<int> maxValueBlood) : base(array, maxValueMain, maxValueBlood) { }
        public Currencies(ACurrencies other, IReactive<int> maxValueMain, IReactive<int> maxValueBlood) : base(other, maxValueMain, maxValueBlood) { }
        public Currencies(IReactive<int> maxValueMain, IReactive<int> maxValueBlood) : base(maxValueMain, maxValueBlood) { }
        public Currencies() : base() { }
        #endregion

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
        
        public void AddBlood(int value) => Amount += _values[CurrencyId.Blood].Add(value);

        public void AddFrom(ACurrencies other)
        {
            if (other.Amount == 0)
                return;
            
            for (int i = 0; i < countAll; i++)
                _amount += _values[i].Add(other[i]);

            actionAmountChange?.Invoke(_amount);
        }

        public void Pay(ACurrencies cost)
        {
            for (int i = 0; i < countAll; i++)
                _amount += _values[i].Add(-cost[i]);

            actionAmountChange?.Invoke(_amount);
        }

        public void ClampMain()
        {
            if (_amount <= _maxMain)
                return;

            int indexMax = 0, index;
            ACurrency max = _values[indexMax], temp;
            for (index = 1; index < countMain; index++)
            {
                temp = _values[index];
                if (temp > max | (temp == max && Chance.Rolling()))
                {
                    indexMax = index;
                    max = temp;
                }
            }

            index = indexMax;
            while (_amount > _maxMain)
            {
                _amount += max.DecrementNotSignal();
                do
                {
                    index = ++index % countMain;
                    temp = _values[index];
                    if (temp > max)
                    {
                        indexMax = index;
                        max = temp;
                    }
                }
                while (index != indexMax);
            }

            for (index = 0; index < countMain; index++)
                _values[index].Signal();

            actionAmountChange?.Invoke(_amount);
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
