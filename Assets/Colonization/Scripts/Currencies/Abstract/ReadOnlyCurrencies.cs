using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    using static CurrencyId;

    public abstract class ReadOnlyCurrencies : ACurrencies, IReactive<ACurrencies>
    {
        protected ACurrency[] _values = new ACurrency[AllCount];
        protected RInt _amount = new(0);
        protected Ability _maxAmount, _maxBlood;
        protected readonly VAction<ACurrencies> _changeEvent = new();

        sealed public override int Amount => _amount.Value;
        public IReactiveValue<int> CurrentAmount => _amount;
        public IReactiveValue<int> MaxAmount => _maxAmount;
        public IReactiveValue<int> MaxBlood => _maxBlood;

        public bool IsOverResources => _maxAmount.Value < _amount.Value;
        sealed public override bool IsEmpty => _amount == 0 & _values[Blood].Value == 0;

        sealed public override int this[int index] { get => _values[index].Value; }
        sealed public override int this[Id<CurrencyId> id] { get => _values[id.Value].Value; }

        #region Constructions
        protected ReadOnlyCurrencies(ACurrencies other, Ability maxMainValue, Ability maxBloodValue)
        {
            _maxAmount = maxMainValue;
            _maxBlood = maxBloodValue;

            for (int i = 0; i < MainCount; i++)
                _values[i] = new MainCurrency(other[i]);

            _values[Blood] = new BloodCurrency(other[Blood], maxBloodValue);

            _amount.SilentValue = other.Amount;
        }
        #endregion

        public Subscription Subscribe(Action<ACurrencies> action, bool instantGetValue = true) => _changeEvent.Add(action, instantGetValue, this);

        public Currency Get(int index) => _values[index];
        public Currency Get(Id<CurrencyId> id) => _values[id.Value];

        sealed public override IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < AllCount; i++)
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
                    _changeValue.Invoke(_value);
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

                    _changeValue.Invoke(value);
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

            public static int operator +(ACurrency currency, int value) => currency.Add(value);
            public static int operator -(ACurrency currency, int value) => currency.Remove(value);
        }
        #endregion
     }
}
