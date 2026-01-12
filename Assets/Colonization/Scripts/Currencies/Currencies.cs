using Vurbiri.Colonization.Storage;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    sealed public class Currencies : ReadOnlyCurrencies
    {
        public new int this[int index] { [Impl(256)] get => _values[index].Value; [Impl(256)] set => _values[index].Set(value); }
        public new int this[Id<CurrencyId> id] { [Impl(256)] get => _values[id.Value].Value; [Impl(256)] set => _values[id.Value].Set(value); }

        private Currencies(StartCurrencies other, Ability maxMainValue, Ability maxBloodValue) : base(other, maxMainValue, maxBloodValue) { }

        public static Currencies Create(ReadOnlyAbilities<HumanAbilityId> abilities, StartCurrencies resDefault, HumanLoadData loadData)
        {
            if (loadData.isLoaded & loadData.resources != null)
                resDefault = loadData.resources;
            return new(resDefault, abilities[HumanAbilityId.MaxMainResources], abilities[HumanAbilityId.MaxBlood]);
        }

        public void Add(ReadOnlyLiteCurrencies other)
        {
            if (other.IsNotEmpty)
            {
                for (int i = 0; i < CurrencyId.Count; ++i)
                    _values[i].Add(other[i]);

                _amount.Add(other.Amount);
                _changeEvent.Invoke(this);
            }
        }
        public void Remove(ReadOnlyLiteCurrencies other)
        {
            if (other.IsNotEmpty)
            {
                for (int i = 0; i < CurrencyId.Count; ++i)
                    _values[i].Remove(other[i]);

                _amount.Remove(other.Amount);
                _changeEvent.Invoke(this);
            }
        }

        public void Add(Id<CurrencyId> currencyId, int value)
        {
            if (value != 0)
            {
                _amount.Value += _values[currencyId].Add(value);
                _changeEvent.Invoke(this);
            }
        }
        public void Remove(Id<CurrencyId> currencyId, int value)
        {
            if (value != 0)
            {
                _amount.Value += _values[currencyId].Remove(value);
                _changeEvent.Invoke(this);
            }
        }

        public void Add(int value)
        {
            if (value != 0)
            {
                for (int i = 0; i < CurrencyId.Count; ++i)
                    _values[i].Add(value);

                _amount.Add(value * CurrencyId.Count);
                _changeEvent.Invoke(this);
            }
        }
        public void Remove(int value)
        {
            if (value != 0)
            {
                for (int i = 0; i < CurrencyId.Count; ++i)
                    _values[i].Remove(value);

                _amount.Remove(value * CurrencyId.Count);
                _changeEvent.Invoke(this);
            }
        }

        public int Clamp()
        {
            int delta = _amount.Value - _maxAmount.Value, output = delta;

            if (delta > 0)
            {
                var values = ConvertToInt(_values);
                int maxIndex = 0, remove;
                while (delta > 0)
                {
                    maxIndex = FindMaxIndex(values, maxIndex);
                    remove = delta / CurrencyId.Count + 1;
                    values[maxIndex] -= remove;
                    delta -= remove;
                }

                for (int index = 0; index < CurrencyId.Count; index++)
                    _values[index].Set(values[index]);

                _amount.Value = _maxAmount.Value;
                _changeEvent.Invoke(this);
            }

            return output;

            #region Local: ConvertToInt(..), FindMaxIndex(..)
            //==============================================
            static int[] ConvertToInt(Currency[] values)
            {
                int[] array = new int[CurrencyId.Count];
                for (int i = 0; i < CurrencyId.Count; ++i)
                    array[i] = values[i].Value;

                return array;
            }
            //==============================================
            static int FindMaxIndex(int[] values, int startIndex)
            {
                int index, maxIndex = startIndex, step = 0;
                while(++step < CurrencyId.Count)
                {
                    index = (startIndex + step) % CurrencyId.Count;
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
            int j = UnityEngine.Random.Range(0, CurrencyId.Count);
            _values[j].Add(1);
            _amount.Add(1);
            _changeEvent.Invoke(this);
        }

        public void RandomDecrement()
        {
            if(_amount.Value > 0)
            {
                int j = UnityEngine.Random.Range(0, CurrencyId.Count);
                while(_values[j] == 0)
                    j = ++j % CurrencyId.Count;
                
                _values[j].Remove(1);
                _amount.Remove(1);
                _changeEvent.Invoke(this);
            }
        }
    }
}
