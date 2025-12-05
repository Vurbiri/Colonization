using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    using static CurrencyId;

    public abstract class ReadOnlyCurrencies : ACurrencies, IReactive<ACurrencies>
    {
        protected ACurrency[] _values = new ACurrency[AllCount];
        protected RInt _amount = new(0);
        protected Ability _maxAmount, _maxBlood;
        protected readonly VAction<ACurrencies> _changeEvent = new();

        sealed public override int Amount { [Impl(256)] get => _amount; }
        public ReactiveValue<int> CurrentAmount { [Impl(256)] get => _amount; }
        public ReactiveValue<int> MaxAmount { [Impl(256)] get => _maxAmount; }
        public int PercentAmount { [Impl(256)] get => (100 * _amount) / _maxAmount; }
        public ReactiveValue<int> MaxBlood { [Impl(256)] get => _maxBlood; }
        public int PercentBlood { [Impl(256)] get => (100 * _values[Blood]) / _maxBlood; }

        public bool IsOverResources => _maxAmount.Value < _amount.Value;
        sealed public override bool IsEmpty => _amount == 0 & _values[Blood].Value == 0;

        sealed public override int this[int index] { [Impl(256)] get => _values[index].Value; }
        sealed public override int this[Id<CurrencyId> id] { [Impl(256)] get => _values[id.Value].Value; }

        #region Min/Max
        public int MinIndex
        {
            get
            {
                int minId = 0;
                for (int i = 1; i < MainCount; ++i)
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
                for (int i = 1; i < MainCount; ++i)
                    if (_values[i] > _values[maxId])
                        maxId = i;
                return maxId;
            }
        }
        #endregion

        #region Constructions
        protected ReadOnlyCurrencies(ACurrencies other, Ability maxMainValue, Ability maxBloodValue)
        {
            _maxAmount = maxMainValue;
            _maxBlood = maxBloodValue;

            for (int i = 0; i < MainCount; ++i)
                _values[i] = new MainCurrency(other[i]);

            _values[Blood] = new BloodCurrency(other[Blood], maxBloodValue);

            _amount.SilentValue = other.Amount;
        }
        #endregion

        [Impl(256)] public Subscription Subscribe(Action<ACurrencies> action, bool instantGetValue = true) => _changeEvent.Add(action, this, instantGetValue);

        [Impl(256)] public Currency Get(int index) => _values[index];
        [Impl(256)] public Currency Get(Id<CurrencyId> id) => _values[id.Value];

        [Impl(256)] public int PercentAmountExCurrency(int currencyId)
        {
            int currency = _values[currencyId].Value;
            return 100 * (_amount - currency) / (_maxAmount - currency);
        }

        public int OverCount(ReadOnlyMainCurrencies values, out int lastIndex)
        {
            int count = 0; lastIndex = -1;
            for (int i = 0; i < MainCount; ++i)
            {
                if (values[i] > _values[i])
                {
                    ++count; lastIndex = i;
                }
            }
            return count;
        }
        public int Deficit(ReadOnlyMainCurrencies values)
        {
            int delta = 0;
            for (int i = 0, cost; i < MainCount; ++i)
            {
                cost = _values[i] - values[i];
                if (cost < 0)
                    delta += cost;
            }
            return delta;
        }

        sealed public override IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < AllCount; ++i)
                yield return _values[i].Value;
        }

        #region Nested: ACurrency, MainCurrency, BloodCurrency
        //*******************************************************
        sealed protected class MainCurrency : ACurrency
        {
            public MainCurrency() : base(0) { }
            public MainCurrency(int value) : base(value) { }

            public override int Add(int delta)
            {
                if (delta != 0)
                {
                    _value += delta;
                    _changeEvent.Invoke(_value);
                    _deltaValue.Invoke(delta);
                }
                return delta;
            }
            public override int Remove(int delta) => Add(-delta);

            public override int Set(int value) => Add(value - _value);
        }
        //*******************************************************
        sealed protected class BloodCurrency : ACurrency
        {
            private readonly Ability _max;
            
            public BloodCurrency() : base(0) { }
            public BloodCurrency(Ability maxValue) : this(0, maxValue) { }
            public BloodCurrency(int value, Ability maxValue) : base(value)
            {
                _max = maxValue;
            }

            public override int Set(int value)
            {
                value = Mathf.Clamp(value, 0, _max.Value);
                if (value != _value)
                {
                    int delta = value - _value;
                    _value = value;

                    _changeEvent.Invoke(value);
                    _deltaValue.Invoke(delta);
                }
                return 0;
            }

            public override int Add(int delta) => Set(_value + delta);
            public override int Remove(int delta) => Set(_value - delta);
        }
        //*******************************************************
        protected abstract class ACurrency : Currency
        {
            public ACurrency(int value) => _value = value;

            public abstract int Set(int value);
            public abstract int Add(int value);
            public abstract int Remove(int value);

            public static int operator +(ACurrency currency, int value) => currency._value + value;
            public static int operator +(int value, ACurrency currency) => value + currency._value;
            public static int operator -(ACurrency currency, int value) => currency._value - value;
            public static int operator -(int value, ACurrency currency) => value - currency._value;

            public static int operator *(ACurrency a, ACurrency b) => a._value * b._value;
        }
        #endregion
     }
}
