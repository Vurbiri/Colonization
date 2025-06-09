using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    using static CurrencyId;

    public abstract class ACurrenciesReactive : ACurrencies, IReactive<ACurrencies>
    {
        protected ACurrency[] _values = new ACurrency[CountAll];
        protected RInt _amount = new(0);
        protected Ability _maxAmount, _maxBlood;
        protected readonly Subscription<ACurrencies> _eventChanged = new();

        public override int Amount => _amount.Value;
        public IReactiveValue<int> CurrentAmount => _amount;
        public IReactiveValue<int> MaxAmount => _maxAmount;
        public IReactive<int, int> Blood => _values[CurrencyId.Blood];
        public IReactiveValue<int> MaxBlood => _maxBlood;

        public override int this[int index] { get => _values[index].Value; }
        public override int this[Id<CurrencyId> id] { get => _values[id.Value].Value; }

        #region Constructions
        public ACurrenciesReactive(IReadOnlyList<int> array, Ability maxValueMain, Ability maxValueBlood)
        {
            _maxAmount = maxValueMain;
            _maxBlood = maxValueBlood;

            int value, amount = 0;
            for (int i = 0; i < CountMain; i++)
            {
                value = array[i];
                _values[i] = new CurrencyMain(value);
                amount += value;
            }
            _values[CurrencyId.Blood] = new CurrencyBlood(array[CurrencyId.Blood], maxValueBlood);

            _amount.SilentValue = amount;
        }
        public ACurrenciesReactive(ACurrencies other, Ability maxValueMain, Ability maxValueBlood)
        {
            _maxAmount = maxValueMain;
            _maxBlood = maxValueBlood;

            for (int i = 0; i < CountMain; i++)
                _values[i] = new CurrencyMain(other[i]);

            _values[CurrencyId.Blood] = new CurrencyBlood(other[CurrencyId.Blood], maxValueBlood);

            _amount.SilentValue = other.Amount;
        }

        public ACurrenciesReactive(Ability maxValueMain, Ability maxValueBlood)
        {
            _maxAmount = maxValueMain;
            _maxBlood = maxValueBlood;

            for (int i = 0; i < CountMain; i++)
                _values[i] = new CurrencyMain();

            _values[CurrencyId.Blood] = new CurrencyBlood(maxValueBlood);
        }
        #endregion

        #region Reactive
        public Unsubscription Subscribe(Action<ACurrencies> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, this);
        public Unsubscription Subscribe(int index, Action<int, int> action, bool instantGetValue = true) => _values[index].Subscribe(action, instantGetValue);
        public Unsubscription Subscribe(Id<CurrencyId> id, Action<int, int> action, bool instantGetValue = true) => _values[id.Value].Subscribe(action, instantGetValue);
        #endregion

        public override IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < CountAll; i++)
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
                    _changeValue.Invoke(_value, delta);
                }
                return delta;
            }
            public override int Add(int delta)
            {
                if (delta != 0)
                {
                    _value += delta;
                    _changeValue.Invoke(_value, delta);
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
                    _changeValue.Invoke(_value, delta);
                }
                return 0;
            }

            public override int Add(int delta) => Set(_value + delta);
        }
        //*******************************************************
        protected abstract class ACurrency : IReactive<int, int>, IEquatable<ACurrency>, IComparable<ACurrency>
        {
            protected int _value;
            protected readonly Subscription<int, int> _changeValue = new();

            public int Value => _value;

            public ACurrency(int value) => _value = value;

            public abstract int Set(int value);
            public abstract int Add(int value);

            public Unsubscription Subscribe(Action<int, int> action, bool instantGetValue = true) => _changeValue.Add(action, instantGetValue, _value, 0);
           
            public static explicit operator int(ACurrency currency) => currency._value;

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
