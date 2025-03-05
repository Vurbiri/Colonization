//Assets\Colonization\Scripts\Currencies\Currencies.cs
using System.Collections.Generic;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
    public class Currencies : ACurrenciesReactive
    {
        #region Constructions
        public Currencies(IReadOnlyList<int> array, IAbility maxValueMain, IAbility maxValueBlood) : base(array, maxValueMain, maxValueBlood) { }
        public Currencies(ACurrencies other, IAbility maxValueMain, IAbility maxValueBlood) : base(other, maxValueMain, maxValueBlood) { }
        public Currencies(IAbility maxValueMain, IAbility maxValueBlood) : base(maxValueMain, maxValueBlood) { }
        #endregion

        public void Add(int index, int value)
        {
            if (value == 0)
                return;

            _amount.Value += _values[index].Add(value);
            _subscriber.Invoke(this);
        }
        public void Add(Id<CurrencyId> id, int value) => Add(id.Value, value);

        public void AddBlood(int value)
        {
            if (value == 0)
                return;

            _values[CurrencyId.Blood].Add(value);
            _subscriber.Invoke(this);
        }

        public void AddFrom(ACurrencies other)
        {
            if (other.Amount == 0)
                return;

            for (int i = 0; i < countAll; i++)
                _values[i].Add(other[i]);

            _amount.Value += other.Amount;
            _subscriber.Invoke(this);
        }

        public void Pay(ACurrencies cost)
        {
            if (cost.Amount == 0)
                return;

            int amount = _amount.Value;
            for (int i = 0; i < countAll; i++)
                amount += _values[i].Add(-cost[i]);

            _amount.Value = amount;
            _subscriber.Invoke(this);
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
                amount += max.SilentDecrement();
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
            _subscriber.Invoke(this);
        }

        public void Clear()
        {
            for (int i = 0; i < countAll; i++)
                _values[i].Set(0);

            _amount.Value = 0;
            _subscriber.Invoke(this);
        }
    }
}
