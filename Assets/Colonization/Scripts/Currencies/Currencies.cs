using System.Text;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    using static CurrencyId;

    sealed public class Currencies : ReadOnlyCurrencies
    {
        private Currencies(ACurrencies other, Ability maxMainValue, Ability maxBloodValue) : base(other, maxMainValue, maxBloodValue) { }

        public static Currencies Create(ReadOnlyAbilities<HumanAbilityId> abilities, CurrenciesLite resDefault, HumanLoadData loadData)
        {
            if (loadData.isLoaded & loadData.resources != null)
                resDefault = loadData.resources;
            return new(resDefault, abilities[HumanAbilityId.MaxMainResources], abilities[HumanAbilityId.MaxBlood]);
        }

        public void Add(ReadOnlyMainCurrencies other)
        {
            if (other.IsNotEmpty)
            {
                for (int i = 0; i < MainCount; ++i)
                    _values[i].Add(other[i]);

                _amount.Add(other.Amount);
                _changeEvent.Invoke(this);
            }
        }
        public void Remove(ReadOnlyMainCurrencies other)
        {
            if (other.IsNotEmpty)
            {
                for (int i = 0; i < MainCount; ++i)
                    _values[i].Remove(other[i]);

                _amount.Remove(other.Amount);
                _changeEvent.Invoke(this);
            }
        }

        public void Add(int currencyId, int value)
        {
            if (value != 0)
            {
                _amount.Value += _values[currencyId].Add(value);
                _changeEvent.Invoke(this);
            }
        }
        public void Remove(int currencyId, int value)
        {
            if (value != 0)
            {
                _amount.Value += _values[currencyId].Remove(value);
                _changeEvent.Invoke(this);
            }
        }

        public void AddBlood(int value)
        {
            if (value != 0)
            {
                _values[Blood].Add(value);
                _changeEvent.Invoke(this);
            }
        }
        public void RemoveBlood(int value)
        {
            if (value != 0)
            {
                _values[Blood].Remove(value);
                _changeEvent.Invoke(this);
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
                _changeEvent.Invoke(this);
            }

            #region Local: ConvertToInt(..), FindMaxIndex(..)
            //==============================================
            static int[] ConvertToInt(ACurrency[] values)
            {
                int[] array = new int[MainCount];
                for (int i = 0; i < MainCount; ++i)
                    array[i] = values[i].Value;

                return array;
            }
            //==============================================
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

        public void AddToMin(int value = 1)
        {
            if (value != 0)
            {
                _amount.Value += _values[MinIndex].Add(value);
                _changeEvent.Invoke(this);
            }
        }

        public void RandomIncrement()
        {
            int j = UnityEngine.Random.Range(0, MainCount);
            _values[j].Add(1);
            _amount.Add(1);
            _changeEvent.Invoke(this);
        }

        public void RandomDecrement()
        {
            if(_amount.Value > 0)
            {
                int j = UnityEngine.Random.Range(0, MainCount);
                while(_values[j] == 0)
                    j = ++j % MainCount;
                
                _values[j].Remove(1);
                _amount.Remove(1);
                _changeEvent.Invoke(this);
            }
        }

        public void MainToStringBuilder(StringBuilder sb)
        {
            for (int i = 0; i < MainCount; ++i)
                sb.AppendFormat(TAG.CURRENCY, i, _values[i].ToString());

            sb.Append(" ");
            sb.Append(_amount.ToString()); sb.Append("/"); sb.Append(_maxAmount.ToString());
        }
    }
}
