//Assets\Colonization\Scripts\Currencies\Abstract\ACurrenciesReactive.cs
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
        protected Ability _maxValueMain, _maxValueBlood;
        protected readonly Signer<ACurrencies> _signer = new();

        public override int Amount => _amount.Value;
        public IReactiveValue<int> AmountCurrent => _amount;
        public IReactiveValue<int> AmountMax => _maxValueMain;
        public IReactiveValue<int> BloodCurrent => _values[CurrencyId.Blood];
        public IReactiveValue<int> BloodMax => _maxValueBlood;

        public override int this[int index] { get => _values[index].Value; }
        public override int this[Id<CurrencyId> id] { get => _values[id.Value].Value; }

        #region Constructions
        public ACurrenciesReactive(IReadOnlyList<int> array, Ability maxValueMain, Ability maxValueBlood)
        {
            _maxValueMain = maxValueMain;
            _maxValueBlood = maxValueBlood;

            int value, amount = 0;
            for (int i = 0; i < CountMain; i++)
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
            _maxValueMain = maxValueMain;
            _maxValueBlood = maxValueBlood;

            for (int i = 0; i < CountMain; i++)
                _values[i] = new CurrencyMain(other[i]);

            _values[Blood] = new CurrencyBlood(other[Blood], maxValueBlood);

            _amount.SilentValue = other.Amount;
        }

        public ACurrenciesReactive(Ability maxValueMain, Ability maxValueBlood)
        {
            _maxValueMain = maxValueMain;
            _maxValueBlood = maxValueBlood;

            for (int i = 0; i < CountMain; i++)
                _values[i] = new CurrencyMain();

            _values[Blood] = new CurrencyBlood(maxValueBlood);
        }
        #endregion

        #region Reactive
        public Unsubscriber Subscribe(Action<ACurrencies> action, bool sendCallback = true) => _signer.Add(action, sendCallback, this);
        public Unsubscriber Subscribe(int index, Action<int> action, bool sendCallback = true) => _values[index].Subscribe(action, sendCallback);
        public Unsubscriber Subscribe(Id<CurrencyId> id, Action<int> action, bool sendCallback = true) => _values[id.Value].Subscribe(action, sendCallback);
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
                if (value == _value)
                    return 0;

                int delta = value - _value;
                Value = value;
                return delta;
            }
            public override int Add(int value)
            {
                Value = _value + value;
                return value;
            }

            public override int Increment()
            {
                Value = _value + 1;
                return 1;
            }
            public override int SilentDecrement()
            {
                if (_value == 0)
                    return 0;

                _value -= 1;
                return -1;
            }
        }
        //*******************************************************
        sealed protected class CurrencyBlood : ACurrency
        {
            private readonly Ability _max;

            public override int Value 
            { 
                get => _value; 
                protected set
                {
                    value = Mathf.Clamp(value, 0, _max.Value);
                    if (value != _value)
                        _signer.Invoke(_value = value);
                }
            }

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
                    _signer.Invoke(_value = value);
                return 0;
            }

            public override int Add(int value) => Set(_value + value);
            
            public override int Increment() => Set(_value + 1);

            public override int SilentDecrement()
            {
                if (_value > 0)
                    _value -= 1;
                return 0;
            }

        }
        //*******************************************************
        protected abstract class ACurrency : AReactiveValue<int>, IEquatable<ACurrency>, IComparable<ACurrency>
        {
            protected int _value;

            public override int Value
            {
                get => _value;
                protected set
                {
                    if (value != _value)
                        _signer.Invoke(_value = value);
                }
            }

            public ACurrency(int value) => _value = value;

            public abstract int Set(int value);
            public abstract int Add(int value);

            public abstract int Increment();
            public abstract int SilentDecrement();

            public void Signal() => _signer.Invoke(_value);

           
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
