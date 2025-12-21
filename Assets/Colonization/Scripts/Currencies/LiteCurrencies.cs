using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class ReadOnlyLiteCurrencies : IReadOnlyList<int>
    {
        [SerializeField] protected int[] _values = new int[CurrencyId.Count];
        [SerializeField] protected int _amount;

        protected readonly Version _version = new();

        public int this[int index] { [Impl(256)] get => _values[index]; }
        public int this[Id<CurrencyId> id] { [Impl(256)] get => _values[id]; }

        public int Count { [Impl(256)] get => CurrencyId.Count; }
        public int Amount { [Impl(256)] get => _amount; }
        public bool IsEmpty { [Impl(256)] get => _amount == 0; }
        public bool IsNotEmpty { [Impl(256)] get => _amount != 0; }

        #region Min/Max
        public int MinIndex
        {
            get
            {
                int minId = 0;
                for (int i = 1; i < CurrencyId.Count; ++i)
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
                for (int i = 1; i < CurrencyId.Count; ++i)
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
                for (int i = 1; i < CurrencyId.Count; ++i)
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
                for (int i = 1; i < CurrencyId.Count; ++i)
                    if (_values[i] > maxValue)
                        maxValue = _values[i];
                return maxValue;
            }
        }
        #endregion

        public ReadOnlyLiteCurrencies() { }
        public ReadOnlyLiteCurrencies(int value)
        {
            for (int i = 0; i < CurrencyId.Count; ++i)
                _values[i] = value;
            _amount = value * CurrencyId.Count;
        }
        public ReadOnlyLiteCurrencies(ReadOnlyLiteCurrencies other)
        {
            for (int i = 0; i < CurrencyId.Count; ++i)
                _values[i] = other._values[i];
            _amount = other._amount;
        }

        #region ToText
        public void ToStringBuilder(StringBuilder sb, string hexPlusColor, string hexMinusColor)
        {
            if (_amount != 0)
            {
                for (int i = 0, resource; i < CurrencyId.Count; ++i)
                {
                    resource = _values[i];
                    if (resource != 0)
                        sb.AppendFormat(TAG.COLOR_CURRENCY, i.ToStr(), resource.ToString("+#;-#;0"), resource > 0 ? hexPlusColor : hexMinusColor);
                }
            }
        }
        public void ToStringBuilder(StringBuilder sb)
        {
            for (int i = 0; i < CurrencyId.Count; ++i)
                sb.AppendFormat(TAG.CURRENCY, i, _values[i].ToString("+#;-#;0"));
        }
        public void PlusToStringBuilder(StringBuilder sb)
        {
            if (_amount > 0)
            {
                for (int i = 0, resource; i < CurrencyId.Count; ++i)
                {
                    resource = _values[i];
                    if (resource > 0)
                        sb.AppendFormat(TAG.CURRENCY, i.ToStr(), resource.ToStr());
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
            for (int i = 0; i < CurrencyId.Count; ++i)
            {
                sb.Append("["); sb.Append(_values[i]); sb.Append("]");
            }
            return sb.ToString();
        }
        #endregion

        [Impl(256)] public ArrayEnumerator<int> GetEnumerator() => new(_values, CurrencyId.Count, _version);
        IEnumerator<int> IEnumerable<int>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #region Arithmetic
        public static LiteCurrencies operator -(ReadOnlyCurrencies a, ReadOnlyLiteCurrencies b)
        {
            LiteCurrencies diff = new();
            for (int i = 0; i < CurrencyId.Count; ++i)
                diff._values[i] = a[i] - b._values[i];
            diff._amount = a.Amount - b._amount;

            return diff;
        }
        public static ReadOnlyLiteCurrencies operator -(ReadOnlyLiteCurrencies a, ReadOnlyLiteCurrencies b)
        {
            ReadOnlyLiteCurrencies diff = new();
            for (int i = 0; i < CurrencyId.Count; ++i)
                diff._values[i] = a._values[i] - b._values[i];
            diff._amount = a._amount - b._amount;

            return diff;
        }
        public static ReadOnlyLiteCurrencies operator +(ReadOnlyLiteCurrencies a, ReadOnlyLiteCurrencies b)
        {
            ReadOnlyLiteCurrencies sum = new();
            for (int i = 0; i < CurrencyId.Count; ++i)
                sum._values[i] = a._values[i] + b._values[i];
            sum._amount = a._amount + b._amount;

            return sum;
        }
        public static ReadOnlyLiteCurrencies operator *(ReadOnlyLiteCurrencies currencies, int rate)
        {
            ReadOnlyLiteCurrencies result = new();
            if (currencies._amount != 0 & rate != 0)
            {
                for (int i = 0; i < CurrencyId.Count; ++i)
                    result._values[i] = currencies._values[i] * rate;
                result._amount = currencies._amount * rate;
            }

            return result;
        }
        #endregion

        #region Comparison
        public static bool operator >=(ReadOnlyLiteCurrencies left, ReadOnlyCurrencies right)
        {
            if (left._amount < right.Amount)
                return false;

            for (int i = 0; i < CurrencyId.Count; ++i)
                if (left._values[i] < right[i])
                    return false;
            return true;
        }
        public static bool operator <=(ReadOnlyLiteCurrencies left, ReadOnlyCurrencies right)
        {
            for (int i = 0; i < CurrencyId.Count; ++i)
                if (left._values[i] > right[i])
                    return false;
            return true;
        }

        public static bool operator >(ReadOnlyLiteCurrencies left, ReadOnlyCurrencies right) => !(left <= right);
        public static bool operator <(ReadOnlyLiteCurrencies left, ReadOnlyCurrencies right) => !(left >= right);

        public static bool operator >=(ReadOnlyCurrencies left, ReadOnlyLiteCurrencies right)
        {
            if (left.Amount < right._amount)
                return false;

            for (int i = 0; i < CurrencyId.Count; ++i)
                if (left[i] < right._values[i])
                    return false;
            return true;
        }
        public static bool operator <=(ReadOnlyCurrencies left, ReadOnlyLiteCurrencies right)
        {
            for (int i = 0; i < CurrencyId.Count; ++i)
                if (left[i] > right._values[i])
                    return false;
            return true;
        }

        public static bool operator >(ReadOnlyCurrencies left, ReadOnlyLiteCurrencies right) => !(left <= right);
        public static bool operator <(ReadOnlyCurrencies left, ReadOnlyLiteCurrencies right) => !(left >= right);
        #endregion
    }

    [System.Serializable]
    public class LiteCurrencies : ReadOnlyLiteCurrencies
    {
        public new int this[int index]
        {
            [Impl(256)] get => _values[index];
            [Impl(256)] set => Set(index, value);
        }
        public new int this[Id<CurrencyId> id]
        {
            [Impl(256)] get => _values[id];
            [Impl(256)] set => Set(id, value);
        }

        [Impl(256)] public LiteCurrencies() { }
        [Impl(256)] public LiteCurrencies(ReadOnlyLiteCurrencies other) : base(other) { }
        public LiteCurrencies(ReadOnlyCurrencies other)
        {
            for (int i = 0; i < CurrencyId.Count; ++i)
                _values[i] = other[i];
            _amount = other.Amount;
        }
        
        [Impl(256)] public void Increment(Id<CurrencyId> id)
        {
            ++_values[id]; ++_amount;
            _version.Next();
        }
        [Impl(256)] public void Decrement(Id<CurrencyId> id)
        {
            --_values[id]; --_amount;
            _version.Next();
        }

        [Impl(256)] public void Set(Id<CurrencyId> id, int value)
        {
            _amount += value - _values[id];
            _values[id] = value;
            _version.Next();
        }

        [Impl(256)] public void Add(Id<CurrencyId> id, int value)
        {
            _amount += value;
            _values[id] += value;
            _version.Next();
        }
        [Impl(256)] public void Remove(Id<CurrencyId> id, int value)
        {
            _amount -= value;
            _values[id] -= value;
            _version.Next();
        }

        [Impl(256)] public void AddToRandom(int value)
        {
            _values[Random.Range(0, CurrencyId.Count)] += value;
            _amount += value;
            _version.Next();
        }

        public void Multiply(int ratio)
        {
            if (_amount != 0)
            {
                for (int i = 0; i < CurrencyId.Count; ++i)
                    _values[i] *= ratio;

                _amount *= ratio;
                _version.Next();
            }
        }

        public void Add(ReadOnlyLiteCurrencies other)
        {
            if (other.IsNotEmpty)
            {
                for (int i = 0; i < CurrencyId.Count; ++i)
                    _values[i] += other[i];
                _amount += other.Amount;
                _version.Next();
            }
        }

        public void RandomAddRange(int count, int maxId)
        {
            _amount += count;
            int sign = count.Sign(); count = sign * count;

            for (int add, index; count > 0; count -= add)
            {
                index = Random.Range(0, maxId);
                add = Random.Range(1, 2 + (count >> 2));
                _values[index] += sign * add;
            }
            _version.Next();
        }

        public void Clear()
        {
            for (int i = 0; i < CurrencyId.Count; ++i)
                _values[i] = 0;
            _amount = 0;
            _version.Next();
        }

        [Impl(256)] public void DirtyReset(int index) => _values[index] = 0;
        [Impl(256)] public void ResetAmount() => _amount = 0;

        public void Normalize(int ratio)
        {
            if (_amount != 0)
            {
                int max = MaxValue;
                for (int i = 0; i < CurrencyId.Count; ++i)
                    _values[i] = (_values[i] - max) * ratio;

                _amount = (_amount - max * CurrencyId.Count) * ratio;
                _version.Next();
            }
        }

        #region Arithmetic
        public static LiteCurrencies operator +(LiteCurrencies a, LiteCurrencies b)
        {
            LiteCurrencies sum = new();
            for (int i = 0; i < CurrencyId.Count; ++i)
                sum._values[i] = a._values[i] + b._values[i];
            sum._amount = a._amount + b._amount;

            return sum;
        }
        public static LiteCurrencies operator -(LiteCurrencies a, LiteCurrencies b)
        {
            LiteCurrencies diff = new();
            for (int i = 0; i < CurrencyId.Count; ++i)
                diff._values[i] = a._values[i] - b._values[i];
            diff._amount = a._amount - b._amount;

            return diff;
        }
        #endregion
    }
}
