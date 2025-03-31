//Assets\Colonization\Scripts\Currencies\Currencies.cs
using System.Collections.Generic;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    using static CurrencyId;

    sealed public class Currencies : ACurrenciesReactive
    {
        #region Constructions
        private Currencies(IReadOnlyList<int> array, Ability maxValueMain, Ability maxValueBlood) : base(array, maxValueMain, maxValueBlood) { }
        private Currencies(ACurrencies other, Ability maxValueMain, Ability maxValueBlood) : base(other, maxValueMain, maxValueBlood) { }
        #endregion

        public static Currencies Create(AbilitiesSet<HumanAbilityId> abilities, PricesScriptable prices, HumanLoadData loadData)
        {
            if (loadData.isLoaded & loadData.resources != null) 
                return new(loadData.resources, abilities[HumanAbilityId.MaxMainResources], abilities[HumanAbilityId.MaxBlood]);
            return new(prices.HumanDefault, abilities[HumanAbilityId.MaxMainResources], abilities[HumanAbilityId.MaxBlood]);
        }

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

            _values[Blood].Add(value);
            _subscriber.Invoke(this);
        }

        public void AddFrom(ACurrencies other)
        {
            if (other.Amount == 0)
                return;

            for (int i = 0; i < CountAll; i++)
                _values[i].Add(other[i]);

            _amount.Add(other.Amount);
            _subscriber.Invoke(this);
        }

        public void Pay(ACurrencies cost)
        {
            if (cost.Amount == 0)
                return;

            int amount = _amount.Value;
            for (int i = 0; i < CountAll; i++)
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
            for (index = 1; index < CountMain; index++)
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
                    index = ++index % CountMain;
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

            for (index = 0; index < CountMain; index++)
                _values[index].Signal();

            _amount.Value = amount;
            _subscriber.Invoke(this);
        }

        public void Clear()
        {
            for (int i = 0; i < CountAll; i++)
                _values[i].Set(0);

            _amount.Value = 0;
            _subscriber.Invoke(this);
        }
    }
}
