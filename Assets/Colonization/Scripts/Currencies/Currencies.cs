using System.Collections.Generic;
using System.Text;
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

        public static Currencies Create(AbilitiesSet<HumanAbilityId> abilities, CurrenciesLite resDefault, HumanLoadData loadData)
        {
            if (loadData.isLoaded & loadData.resources != null) 
                return new(loadData.resources, abilities[HumanAbilityId.MaxMainResources], abilities[HumanAbilityId.MaxBlood]);
            return new(resDefault, abilities[HumanAbilityId.MaxMainResources], abilities[HumanAbilityId.MaxBlood]);
        }

        public void Add(ACurrencies other)
        {
            if (!other.IsEmpty)
            {
                for (int i = 0; i < AllCount; i++)
                    _values[i].Add(other[i]);

                _amount.Add(other.Amount);
                _eventChanged.Invoke(this);
            }
        }
        public void Remove(ACurrencies other)
        {
            if (!other.IsEmpty)
            {
                for (int i = 0; i < AllCount; i++)
                    _values[i].Add(-other[i]);

                _amount.Add(-other.Amount);
                _eventChanged.Invoke(this);
            }
        }

        public void Add(int currencyId, int value)
        {
            if (value != 0)
            {
                _amount.Value += _values[currencyId].Add(value);
                _eventChanged.Invoke(this);
            }
        }
        public void Remove(int currencyId, int value)
        {
            if (value != 0)
            {
                _amount.Value += _values[currencyId].Add(-value);
                _eventChanged.Invoke(this);
            }
        }

        public void AddBlood(int value)
        {
            if (value != 0)
            {
                _values[Blood].Add(value);
                _eventChanged.Invoke(this);
            }
        }
        public void RemoveBlood(int value)
        {
            if (value != 0)
            {
                _values[Blood].Add(-value);
                _eventChanged.Invoke(this);
            }
        }

        public void ClampMain()
        {
            int delta = _amount.Value - _maxAmount.Value;

            if (delta > 0)
            {
                int[] values = ConvertToInt(_values);
                int maxIndex = 0;
                while (delta --> 0)
                {
                    maxIndex = FindMaxIndex(values, maxIndex);
                    values[maxIndex]--;
                }

                for (int index = 0; index < MainCount; index++)
                    _values[index].Set(values[index]);

                _amount.Value = _maxAmount.Value;
                _eventChanged.Invoke(this);
            }

            #region Local: ConvertToInt(), FindMaxIndex()
            //=================================
            static int[] ConvertToInt(ACurrency[] values)
            {
                int[] array = new int[MainCount];
                for (int i = 0; i < MainCount; i++)
                    array[i] = values[i].Value;

                return array;
            }
            //=================================
            static int FindMaxIndex(int[] values, int startIndex)
            {
                int index, maxIndex = startIndex, step = 1;
                for(; step < MainCount; step++)
                {
                    index = (startIndex + step) % MainCount;
                    if (values[index] > values[maxIndex] || (values[index] == values[maxIndex] && Chance.Rolling()))
                        maxIndex = index;
                }

                return maxIndex;
            }
            #endregion
        }

        public void Halving(int currencyId)
        {
            int value = _values[currencyId].Value;
            if (value > 0)
            {
                value >>= 1;
                _amount.Value += _values[currencyId].Set(value);
                _eventChanged.Invoke(this);
            }
        }

        public void MainToStringBuilder(StringBuilder sb)
        {
            for (int i = 0; i < MainCount; i++)
                sb.AppendFormat(TAG.CURRENCY, i, _values[i].ToString());

            sb.Append(" ");
            sb.Append(_amount.ToString()); sb.Append("/"); sb.Append(_maxAmount.ToString());
        }
    }
}
