using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    using static CurrencyId;

    public interface ICurrency : IReactiveValue<int>
    {
        public Unsubscription SubscribeDelta(Action<int> action);
    }

    public abstract class ACurrenciesReactive : ACurrencies, IReactive<ACurrencies>
    {
        protected ACurrency[] _values = new ACurrency[AllCount];
        protected RInt _amount = new(0);
        protected Ability _maxAmount, _maxBlood;
        protected readonly Subscription<ACurrencies> _eventChanged = new();

        public override int Amount => _amount.Value;
        public IReactiveValue<int> CurrentAmount => _amount;
        public IReactiveValue<int> MaxAmount => _maxAmount;
        public IReactiveValue<int> MaxBlood => _maxBlood;

        public bool IsOverResources => _maxAmount.Value < _amount.Value;

        public override int this[int index] { get => _values[index].Value; }
        public override int this[Id<CurrencyId> id] { get => _values[id.Value].Value; }

        #region Constructions
        public ACurrenciesReactive(IReadOnlyList<int> array, Ability maxValueMain, Ability maxValueBlood)
        {
            _maxAmount = maxValueMain;
            _maxBlood = maxValueBlood;

            int value, amount = 0;
            for (int i = 0; i < MainCount; i++)
            {
                value = array[i];
                _values[i] = new CurrencyMain(value);
                amount += value;
            }
            _values[Blood] = new CurrencyBlood(array[Blood], maxValueBlood);

            _amount.SilentValue = amount;
        }
        public ACurrenciesReactive(ACurrencies other, Ability maxValueMain, Ability maxValueBlood)
        {
            _maxAmount = maxValueMain;
            _maxBlood = maxValueBlood;

            for (int i = 0; i < MainCount; i++)
                _values[i] = new CurrencyMain(other[i]);

            _values[Blood] = new CurrencyBlood(other[Blood], maxValueBlood);

            _amount.SilentValue = other.Amount;
        }
        #endregion

        public Unsubscription Subscribe(Action<ACurrencies> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, this);

        public ICurrency Get(int index) => _values[index];
        public ICurrency Get(Id<CurrencyId> id) => _values[id.Value];

        public override IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < AllCount; i++)
                yield return _values[i].Value;
        }

        #region Nested: ACurrency, CurrencyMain, CurrencyBlood
        //*******************************************************
        sealed protected class CurrencyMain : ACurrency
        {
            public CurrencyMain() : base(0) { }
            public CurrencyMain(int value) : base(value) { }

            public override int Set(int value)
            {
                int delta = 0;
                if (value != _value)
                {
                    delta = value - _value;
                    _value = value;
                    _changeValue.Invoke(_value);
                    _deltaValue.Invoke(delta);
                }
                return delta;
            }
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
        }
        //*******************************************************
        sealed protected class CurrencyBlood : ACurrency
        {
            private readonly Ability _max;
            
            public CurrencyBlood() : base(0) { }
            public CurrencyBlood(Ability maxValue) : this(0, maxValue) { }
            public CurrencyBlood(int value, Ability maxValue) : base(value)
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
        }
        //*******************************************************
        protected abstract class ACurrency : ICurrency, IEquatable<ACurrency>, IComparable<ACurrency>
        {
            protected int _value;
            protected readonly Subscription<int> _changeValue = new();
            protected readonly Subscription<int> _deltaValue = new();

            public int Value => _value;

            public ACurrency(int value) => _value = value;

            public abstract int Set(int value);
            public abstract int Add(int value);

            public Unsubscription Subscribe(Action<int> action, bool instantGetValue = true) => _changeValue.Add(action, instantGetValue, _value);
            public Unsubscription SubscribeDelta(Action<int> action) => _deltaValue.Add(action);

            public bool Equals(ACurrency other) => other is not null && _value == other._value;
            public int CompareTo(ACurrency other) => -_value.CompareTo(other._value);
            public override bool Equals(object other) => Equals(other as ACurrency);
            public override int GetHashCode() => _value.GetHashCode();
            public override string ToString() => _value.ToString();

            public static bool operator ==(ACurrency a, ACurrency b) => (a is null & b is null) || (a is not null & b is not null && a._value == b._value);
            public static bool operator !=(ACurrency a, ACurrency b) => !(a == b);
            public static bool operator >=(ACurrency a, ACurrency b) => (a is null & b is null) || (a is not null & b is not null && a._value >= b._value);
            public static bool operator <(ACurrency a, ACurrency b) => !(a >= b);
            public static bool operator <=(ACurrency a, ACurrency b) => (a is null & b is null) || (a is not null & b is not null && a._value <= b._value);
            public static bool operator >(ACurrency a, ACurrency b) => !(a <= b);
        }
        #endregion
     }
}
