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

        public static Currencies Create(AbilitiesSet<HumanAbilityId> abilities, Prices prices, HumanLoadData loadData)
        {
            if (loadData.isLoaded & loadData.resources != null) 
                return new(loadData.resources, abilities[HumanAbilityId.MaxMainResources], abilities[HumanAbilityId.MaxBlood]);
            return new(prices.HumanDefault, abilities[HumanAbilityId.MaxMainResources], abilities[HumanAbilityId.MaxBlood]);
        }

        public void AddFrom(ACurrencies other)
        {
            if (other.Amount == 0)
                return;

            for (int i = 0; i < CountAll; i++)
                _values[i].Add(other[i]);

            _amount.Add(other.Amount);
            _eventChanged.Invoke(this);
        }

        public void AddBlood(int value)
        {
            if (value <= 0)
                return;

            _values[Blood].Add(value);
            _eventChanged.Invoke(this);
        }

        public void Pay(ACurrencies cost)
        {
            if (cost.Amount == 0)
                return;

            int amount = _amount.Value;
            for (int i = 0; i < CountAll; i++)
                amount += _values[i].Add(-cost[i]);

            _amount.Value = amount;
            _eventChanged.Invoke(this);
        }

        public void PayInBlood(int value)
        {
            if (value <= 0)
                return;

            _values[Blood].Add(-value);
            _eventChanged.Invoke(this);
        }

        public void ClampMain()
        {
            int amount = _amount.Value, maxMain = _maxAmount.Value;

            if (amount <= maxMain)
                return;

            int[] values = ConvertToInt(_values);
            int maxIndex = 0;
            do
            {
                maxIndex = FindMaxIndex(values, maxIndex);
                values[maxIndex]--;
            } 
            while (--amount > maxMain);

            for (int index = 0; index < CountMain; index++)
                _values[index].Set(values[index]);

            _amount.Value = amount;
            _eventChanged.Invoke(this);

            #region Local: ConvertToInt(), FindMaxIndex()
            //=================================
            static int[] ConvertToInt(ACurrency[] values)
            {
                int[] array = new int[CountMain];
                for (int i = 0; i < CountMain; i++)
                    array[i] = values[i].Value;

                return array;
            }
            //=================================
            static int FindMaxIndex(int[] values, int maxIndex = 0)
            {
                int index, count = CountMain;
                while (count --> 1)
                {
                    index = (maxIndex + count) % CountMain;
                    if (values[index] > values[maxIndex] || (values[index] == values[maxIndex] && Chance.Rolling()))
                        maxIndex = index;
                }

                return maxIndex;
            }
            #endregion
        }
    }
}
