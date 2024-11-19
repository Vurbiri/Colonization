namespace Vurbiri.Colonization
{
    using System.Collections.Generic;
    using Vurbiri.Reactive;

    public class Currencies : ACurrenciesReactive
    {
        #region Constructions
        public Currencies(IReadOnlyList<int> array, IReadOnlyReactive<int> maxValueMain, IReadOnlyReactive<int> maxValueBlood) : 
            base(array, maxValueMain, maxValueBlood) { }
        public Currencies(ACurrencies other, IReadOnlyReactive<int> maxValueMain, IReadOnlyReactive<int> maxValueBlood) : 
            base(other, maxValueMain, maxValueBlood) { }
        public Currencies(IReadOnlyReactive<int> maxValueMain, IReadOnlyReactive<int> maxValueBlood) : 
            base(maxValueMain, maxValueBlood) { }
        public Currencies() : base() { }
        #endregion

        public void Set(int index, int value) => _amount.Value += _values[index].Set(value);
        public void Set(Id<CurrencyId> id, int value) => _amount.Value += _values[id.Value].Set(value);

        public void Increment(int index) => _amount.Value += _values[index].Increment();
        public void Increment(Id<CurrencyId> id) => _amount.Value += _values[id.Value].Increment();

        public void Add(int index, int value)
        {
            if (value != 0)
                _amount.Value += _values[index].Add(value);
        }
        public void Add(Id<CurrencyId> id, int value) => Add(id.Value, value);
        
        public void AddBlood(int value) => _values[CurrencyId.Blood].Add(value);

        public void AddFrom(ACurrencies other)
        {
            if (other.Amount == 0)
                return;

            for (int i = 0; i < countAll; i++)
                _values[i].Add(other[i]);

            _amount.Value += other.Amount;
        }

        public void Pay(ACurrencies cost)
        {
            int amount = _amount.Value;
            for (int i = 0; i < countAll; i++)
                amount += _values[i].Add(-cost[i]);

            _amount.Value = amount;
        }

        public void ClampMain()
        {
            int amount = _amount.Value, maxMain = _maxValueMain.Value;
            if (amount <= maxMain)
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
            do
            {
                amount += max.DecrementNotSignal();
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
            while (amount > maxMain);

            for (index = 0; index < countMain; index++)
                _values[index].Signal();

            _amount.Value = amount;
        }

        public void Clear()
        {
            for (int i = 0; i < countAll; i++)
                _values[i].Set(0);
            _amount.Value = 0;
        }
    }
}
