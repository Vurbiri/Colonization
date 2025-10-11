using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class ReadOnlyMainCurrencies : IReadOnlyList<int>
    {
        protected const int COUNT = CurrencyId.MainCount;

        [SerializeField] protected int[] _values = new int[COUNT];
        [SerializeField] protected int _amount;

        public int this[int index] { [Impl(256)] get => _values[index]; }
        public int Count { [Impl(256)] get => COUNT; }
        public int Amount { [Impl(256)] get => _amount; }
        public bool IsEmpty { [Impl(256)] get => _amount == 0; }
        public bool IsNotEmpty { [Impl(256)] get => _amount != 0; }

        #region Min/Max
        public int MinIndex
        {
            get
            {
                int minId = 0;
                for (int i = 1; i < COUNT; i++)
                    if (_values[i] < _values[minId])
                        minId = i;
                return minId;
            }
        }
        public int MaxIndex
        {
            get
            {
                int maxId = 0;
                for (int i = 1; i < COUNT; i++)
                    if (_values[i] > _values[maxId])
                        maxId = i;
                return maxId;
            }
        }

        public int MinValue
        {
            get
            {
                int minValue = _values[0];
                for (int i = 1; i < COUNT; i++)
                    if (_values[i] < minValue)
                        minValue = _values[i];
                return minValue;
            }
        }
        public int MaxValue
        {
            get
            {
                int maxValue = _values[0];
                for (int i = 1; i < COUNT; i++)
                    if (_values[i] > maxValue)
                        maxValue = _values[i];
                return maxValue;
            }
        }
        #endregion

        #region ToText
        public void ToStringBuilder(StringBuilder sb, string hexPlusColor, string hexMinusColor)
        {
            if (_amount != 0)
            {
                for (int i = 0, resource; i < COUNT; i++)
                {
                    resource = _values[i];
                    if (resource != 0)
                        sb.AppendFormat(TAG.COLOR_CURRENCY, CONST.NUMBERS_STR[i], resource.ToString("+#;-#;0"), resource > 0 ? hexPlusColor : hexMinusColor);
                }
                sb.Append(TAG.COLOR_OFF);
            }
        }
        public void ToStringBuilder(StringBuilder sb)
        {
            for (int i = 0; i < COUNT; i++)
                sb.AppendFormat(TAG.CURRENCY, i, _values[i].ToString("+#;-#;0"));
        }
        public void PlusToStringBuilder(StringBuilder sb)
        {
            if (_amount > 0)
            {
                for (int i = 0, resource; i < COUNT; i++)
                {
                    resource = _values[i];
                    if (resource > 0)
                        sb.AppendFormat(TAG.CURRENCY, CONST.NUMBERS_STR[i], CONST.NUMBERS_STR[resource]);
                }
            }
        }
        public string PlusToString(string startStr)
        {
            StringBuilder sb = new(startStr);
            PlusToStringBuilder(sb);
            return sb.ToString();
        }
        sealed public override string ToString()
        {
            StringBuilder sb = new();
            for (int i = 0; i < COUNT; i++)
            {
                sb.Append("["); sb.Append(_values[i]); sb.Append("]");
            }
            return sb.ToString();
        }
        #endregion

        public IEnumerator<int> GetEnumerator() => new ArrayEnumerator<int>(_values, COUNT);
        IEnumerator IEnumerable.GetEnumerator() => new ArrayEnumerator<int>(_values, COUNT);

        #region Arithmetic
        public static MainCurrencies operator -(ReadOnlyCurrencies a, ReadOnlyMainCurrencies b)
        {
            MainCurrencies diff = new();
            for (int i = 0; i < COUNT; i++)
                diff._values[i] = a[i] - b._values[i];
            diff._amount = a.Amount - b._amount;

            return diff;
        }
        public static ReadOnlyMainCurrencies operator +(ReadOnlyMainCurrencies a, ReadOnlyMainCurrencies b)
        {
            ReadOnlyMainCurrencies sum = new();
            for (int i = 0; i < COUNT; i++)
                sum._values[i] = a._values[i] + b._values[i];
            sum._amount = a._amount + b._amount;

            return sum;
        }
        public static ReadOnlyMainCurrencies operator *(ReadOnlyMainCurrencies currencies, int rate)
        {
            ReadOnlyMainCurrencies result = new();
            if (currencies._amount != 0 & rate != 0)
            {
                for (int i = 0; i < COUNT; i++)
                    result._values[i] = currencies._values[i] * rate;
                result._amount = currencies._amount * rate;
            }

            return result;
        }
        #endregion

        #region Comparison
        public static bool operator >=(ReadOnlyMainCurrencies left, ReadOnlyCurrencies right)
        {
            if (left._amount < right.Amount)
                return false;

            for (int i = 0; i < COUNT; i++)
                if (left._values[i] < right[i])
                    return false;
            return true;
        }
        public static bool operator <=(ReadOnlyMainCurrencies left, ReadOnlyCurrencies right)
        {
            for (int i = 0; i < COUNT; i++)
                if (left._values[i] > right[i])
                    return false;
            return true;
        }

        public static bool operator >(ReadOnlyMainCurrencies left, ReadOnlyCurrencies right) => !(left <= right);
        public static bool operator <(ReadOnlyMainCurrencies left, ReadOnlyCurrencies right) => !(left >= right);

        public static bool operator >=(ReadOnlyCurrencies left, ReadOnlyMainCurrencies right)
        {
            if (left.Amount < right._amount)
                return false;

            for (int i = 0; i < COUNT; i++)
                if (left[i] < right._values[i])
                    return false;
            return true;
        }
        public static bool operator <=(ReadOnlyCurrencies left, ReadOnlyMainCurrencies right)
        {
            for (int i = 0; i < COUNT; i++)
                if (left[i] > right._values[i])
                    return false;
            return true;
        }

        public static bool operator >(ReadOnlyCurrencies left, ReadOnlyMainCurrencies right) => !(left <= right);
        public static bool operator <(ReadOnlyCurrencies left, ReadOnlyMainCurrencies right) => !(left >= right);
        #endregion
    }


    [System.Serializable]
    public class MainCurrencies : ReadOnlyMainCurrencies
    {
        public new int this[int index]
        {
            [Impl(256)] get => _values[index];
            [Impl(256)] set => Set(index, value);
        }

        public MainCurrencies() { }
        public MainCurrencies(ReadOnlyCurrencies other)
        {
            for (int i = 0; i < COUNT; i++)
                _values[i] = other[i];
            _amount = other.Amount;
        }
        public MainCurrencies(ReadOnlyMainCurrencies other)
        {
            for (int i = 0; i < COUNT; i++)
                _values[i] = other[i];
            _amount = other.Amount;
        }
        public MainCurrencies(MainCurrencies other)
        {
            for (int i = 0; i < COUNT; i++)
                _values[i] = other._values[i];
            _amount = other._amount;
        }

        [Impl(256)] public void Increment(int index)
        {
            _values[index]++; _amount++;
        }
        [Impl(256)] public void Decrement(int index)
        {
            _values[index]--; _amount--;
        }

        [Impl(256)] public void Set(int index, int value)
        {
            _amount += value - _values[index];
            _values[index] = value;
        }
        [Impl(256)] public void Add(int index, int value)
        {
            _amount += value;
            _values[index] += value;
        }
        [Impl(256)] public void Remove(int index, int value)
        {
            _amount -= value;
            _values[index] -= value;
        }

        [Impl(256)] public void AddToRandom(int value)
        {
            _values[Random.Range(0, COUNT)] += value;
            _amount += value;
        }

        public void Multiply(int ratio)
        {
            if (_amount != 0)
            {
                for (int i = 0; i < COUNT; i++)
                    _values[i] *= ratio;

                _amount *= ratio;
            }
        }

        public void Add(ReadOnlyMainCurrencies other)
        {
            if (other.IsNotEmpty)
            {
                for (int i = 0; i < COUNT; i++)
                    _values[i] += other[i];
                _amount += other.Amount;
            }
        }

        public void RandomAddRange(int count, int maxId = COUNT)
        {
            _amount += count;
            int sign = count < 0 ? -1 : 1; count = sign * count;

            for (int add, index; count > 0; count -= add)
            {
                index = Random.Range(0, maxId);
                add = Random.Range(1, 2 + (count >> 2));
                _values[index] += sign * add;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < COUNT; i++)
                _values[i] = 0;
            _amount = 0;
        }

        [Impl(256)] public void DirtyReset(int index) => _values[index] = 0;
        [Impl(256)] public void ResetAmount() => _amount = 0;

        public void Import(ReadOnlyCurrencies other)
        {
            for (int i = 0; i < COUNT; i++)
                _values[i] = other[i];
            _amount = other.Amount;
        }

        public void Normalize(int ratio)
        {
            if (_amount != 0)
            {
                int max = MaxValue;
                for (int i = 0; i < COUNT; i++)
                    _values[i] = (_values[i] - max) * ratio;

                _amount = (_amount - max * COUNT) * ratio;
            }
        }

        #region Arithmetic
        public static MainCurrencies operator +(MainCurrencies a, MainCurrencies b)
        {
            MainCurrencies sum = new();
            for (int i = 0; i < COUNT; i++)
                sum._values[i] = a._values[i] + b._values[i];
            sum._amount = a._amount + b._amount;

            return sum;
        }
        public static MainCurrencies operator -(MainCurrencies a, MainCurrencies b)
        {
            MainCurrencies diff = new();
            for (int i = 0; i < COUNT; i++)
                diff._values[i] = a._values[i] - b._values[i];
            diff._amount = a._amount - b._amount;

            return diff;
        }
        #endregion
    }
}
