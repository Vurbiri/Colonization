using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;
using Random = UnityEngine.Random;

namespace Vurbiri.Colonization
{
    using static CurrencyId;

    [Serializable]
    sealed public class CurrenciesLite : ACurrencies
    {
        [SerializeField] private int[] _values = new int[AllCount];
        [SerializeField] private int _amount = 0;

        public override int Amount { [Impl(256)] get => _amount; }
        public override bool IsEmpty { [Impl(256)] get => _amount == 0 & _values[Blood] == 0; }

        public override int this[int index] { [Impl(256)] get => _values[index];}
        public override int this[Id<CurrencyId> id] { [Impl(256)] get => _values[id.Value]; }

        public CurrenciesLite() { }
        public CurrenciesLite(int[] array)
        {
            for (int i = 0; i < MainCount; i++)
            {
                _values[i] = array[i];
                _amount += array[i];
            }
            _values[Blood] = array[Blood];
        }
        public CurrenciesLite(CurrenciesLite other)
        {
            for (int i = 0; i < AllCount; i++)
                _values[i] = other._values[i];
            _amount = other._amount;
        }

        [Impl(256)] public void IncrementMain(int index)
        {
            _values[index]++; _amount++;
        }
        [Impl(256)] public void DecrementMain(int index)
        {
            _values[index]--; _amount--;
        }

        [Impl(256)] public void Set(int index, int value)
        {
            if (index != Blood)
                _amount += value - _values[index];

            _values[index] = value;
        }

        [Impl(256)] public void Add(int index, int value)
        {
            if (index != Blood)
                _amount += value;

            _values[index] += value;
        }

        [Impl(256)] public void SetMain(int index, int value)
        {
            _amount += value - _values[index];
            _values[index] = value;
        }
        [Impl(256)] public void AddMain(int index, int value)
        {
            _amount += value;
            _values[index] += value;
        }

        [Impl(256)] public void SetBlood(int value) => _values[Blood] = value;
        [Impl(256)] public void AddBlood(int value) => _values[Blood] += value;

        public void Add(CurrenciesLite other)
        {
            if (other._amount != 0)
            {
                for (int i = 0; i < MainCount; i++)
                    _values[i] += other._values[i];
                _amount += other._amount;
            }
            _values[Blood] += other._values[Blood];
        }

        public void MultiplyMain(int ratio)
        {
            if (_amount != 0)
            {
                for (int i = 0; i < MainCount; i++)
                    _values[i] *= ratio;

                _amount *= ratio;
            }
        }

        [Impl(256)] public void RandomAddMain(int value)
        {
            _values[Random.Range(0, MainCount)] += value;
            _amount += value;
        }

        public void RandomAddRange(int count, int maxId = MainCount)
        {
            _amount += count;
            int sign = count < 0 ? -1 : 1; count = sign * count;

            for (int add, index; count > 0; count -= add)
            {
                index = Random.Range(0, maxId);
                add   = Random.Range(1, 2 + (count >> 2));
                _values[index] += sign * add;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < AllCount; i++)
                _values[i] = 0;

            _amount = 0;
        }

        [Impl(256)] public void DirtyReset(int index) => _values[index] = 0;
        [Impl(256)] public void ResetAmount() => _amount = 0;

        public void MainToStringBuilder(StringBuilder sb, string hexPlusColor, string hexMinusColor)
        {
            if (_amount != 0)
            {
                for (int i = 0, resource; i < MainCount; i++)
                {
                    resource = _values[i];
                    if (resource != 0)
                        sb.AppendFormat(TAG.COLOR_CURRENCY, CONST.NUMBERS_STR[i], resource.ToString("+#;-#;0"), resource > 0 ? hexPlusColor : hexMinusColor);
                }
                sb.Append(TAG.COLOR_OFF);
            }
        }
        public void MainPlusToStringBuilder(StringBuilder sb)
        {
            if (_amount > 0)
            {
                for (int i = 0, resource; i < MainCount; i++)
                {
                    resource = _values[i];
                    if (resource > 0)
                        sb.AppendFormat(TAG.CURRENCY, CONST.NUMBERS_STR[i], CONST.NUMBERS_STR[resource]);
                }
            }
        }
        public string MainPlusToString(int countLine)
        {
            StringBuilder sb = new();
            while (countLine --> 0) sb.AppendLine();
            MainPlusToStringBuilder(sb);
            return sb.ToString();
        }
        public string PlusToString(int countLine)
        {
            if (_amount > 0 | _values[Blood] != 0)
            {
                StringBuilder sb = new();
                while (countLine-- > 0) sb.AppendLine();
                for (int i = 0, resource; i < AllCount; i++)
                {
                    resource = _values[i];
                    if (resource > 0)
                        sb.AppendFormat(TAG.CURRENCY, CONST.NUMBERS_STR[i], CONST.NUMBERS_STR[resource]);
                }
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        public void MainToStringBuilder(StringBuilder sb)
        {
            for (int i = 0; i < MainCount; i++)
                sb.AppendFormat(TAG.CURRENCY, i, _values[i].ToString("+#;-#;0"));
        }

        public override IEnumerator<int> GetEnumerator() => new ArrayEnumerator<int>(_values, AllCount);

        public static CurrenciesLite operator +(CurrenciesLite a, CurrenciesLite b)
        {
            if(a == null | b == null) return null;

            if (a._amount == 0) return new(b);
            if (b._amount == 0) return new(a);

            CurrenciesLite sum = new();
            for (int i = 0; i < AllCount; i++)
                sum._values[i] = a._values[i] + b._values[i];
            sum._amount = a._amount + b._amount;

            return sum;
        }
        public static CurrenciesLite operator -(CurrenciesLite a, CurrenciesLite b)
        {
            if (a == null | b == null) return null;

            if (a._amount == 0) return -b;
            if (b._amount == 0) return new(a);

            CurrenciesLite diff = new();
            for (int i = 0; i < AllCount; i++)
                diff._values[i] = a._values[i] - b._values[i];
            diff._amount = a._amount - b._amount;

            return diff;
        }

        public static CurrenciesLite operator -(CurrenciesLite a)
        {
            if (a == null) return null;
            if (a._amount == 0) return new();

            CurrenciesLite neg = new();
            for (int i = 0; i < AllCount; i++)
                neg._values[i] = -a._values[i];
            neg._amount = -a._amount;

            return neg;
        }
    }
}
