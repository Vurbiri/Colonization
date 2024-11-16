using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public abstract class AReadOnlyCurrenciesReactive : ACurrencies, IReactiveSubValues<int, CurrencyId>, IDisposable
    {
        protected ACurrency[] _values = new ACurrency[countAll];
        protected Action<int> actionAmountChange;
        protected int _maxMain;
        private readonly IUnsubscriber _subscriber;

        public override int Amount
        {
            get => _amount;
            protected set { if (_amount != value) actionAmountChange?.Invoke(_amount = value); }
        }

        public override int this[int index] { get => _values[index].Value; }
        public override int this[Id<CurrencyId> id] { get => _values[id.Value].Value; }

        #region Constructions
        public AReadOnlyCurrenciesReactive(IReadOnlyList<int> array, IReactive<int> maxValueMain, IReactive<int> maxValueBlood)
        {
            _subscriber = maxValueMain.Subscribe(v => _maxMain = v);

            int value;
            for (int i = 0; i < countMain; i++)
            {
                value = array[i];
                _values[i] = new CurrencyMain(value);
                _amount += value;
            }
            _values[CurrencyId.Blood] = new CurrencyBlood(array[CurrencyId.Blood], maxValueBlood);
        }
        public AReadOnlyCurrenciesReactive(ACurrencies other, IReactive<int> maxValueMain, IReactive<int> maxValueBlood)
        {
            _subscriber = maxValueMain.Subscribe(v => _maxMain = v);

            for (int i = 0; i < countMain; i++)
                _values[i] = new CurrencyMain(other[i]);
            _values[CurrencyId.Blood] = new CurrencyBlood(other[CurrencyId.Blood], maxValueBlood);

            _amount = other.Amount;
        }
        public AReadOnlyCurrenciesReactive(IReactive<int> maxValueMain, IReactive<int> maxValueBlood)
        {
            _subscriber = maxValueMain.Subscribe(v => _maxMain = v);

            for (int i = 0; i < countMain; i++)
                _values[i] = new CurrencyMain();
            _values[CurrencyId.Blood] = new CurrencyBlood(maxValueBlood);
        }

        public AReadOnlyCurrenciesReactive()
        {
            for (int i = 0; i < countMain; i++)
                _values[i] = new CurrencyMain();
            _values[CurrencyId.Blood] = new CurrencyBlood();
        }
        #endregion

        #region Reactive
        public IUnsubscriber Subscribe(Action<int> action, bool calling = true)
        {
            actionAmountChange -= action ?? throw new ArgumentNullException("action");

            actionAmountChange += action;
            if (calling)
                action(_amount);

            return new Unsubscriber<int>(this, action);
        }
        public IUnsubscriber Subscribe(int index, Action<int> action, bool calling = true) => _values[index].Subscribe(action, calling);
        public IUnsubscriber Subscribe(Id<CurrencyId> id, Action<int> action, bool calling = true) => _values[id.Value].Subscribe(action, calling);
        public void Unsubscribe(Action<int> action) => actionAmountChange -= action ?? throw new ArgumentNullException("action");
        public void Unsubscribe(int index, Action<int> action) => _values[index].Unsubscribe(action);
        public void Unsubscribe(Id<CurrencyId> id, Action<int> action) => _values[id.Value].Unsubscribe(action);
        #endregion

        public void Dispose()
        {
            _subscriber?.Unsubscribe();
            for (int i = 0; i < countAll; i++)
                _values[i].Dispose();
        }

        #region Nested: ACurrency, CurrencyMain, CurrencyBlood
        //*******************************************************
        protected class CurrencyMain : ACurrency
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
            public override int DecrementNotSignal()
            {
                if (_value == 0)
                    return 0;

                _value -= 1;
                return -1;
            }
        }
        //*******************************************************
        protected class CurrencyBlood : ACurrency
        {
            private int _max;
            private readonly IUnsubscriber _subscriber;

            public override int Value 
            { 
                get => _value; 
                protected set
                {
                    if (value != _value)
                        actionValueChange?.Invoke(_value = Mathf.Clamp(value, 0, _max));
                }
            }

            public CurrencyBlood() : base(0) { }
            public CurrencyBlood(IReactive<int> maxValue) : this(0, maxValue) { }
            public CurrencyBlood(int value, IReactive<int> maxValue) : base(value)
            {
                _subscriber = maxValue.Subscribe(v => _max = v);
            }

            public override int Set(int value)
            {
                if (value != _value)
                    actionValueChange?.Invoke(_value = Mathf.Clamp(value, 0, _max));
                return 0;
            }

            public override int Add(int value) => Set(_value + value);
            
            public override int Increment() => Set(_value + 1);

            public override int DecrementNotSignal()
            {
                if (_value > 0)
                    _value -= 1;
                return 0;
            }

            public override void Dispose()
            {
                _subscriber?.Unsubscribe();
            }
        }
        //*******************************************************
        protected abstract class ACurrency : AReactive<int>, IDisposable, IEquatable<ACurrency>, IComparable<ACurrency>
        {
            protected int _value;

            public override int Value
            {
                get => _value;
                protected set
                {
                    if (value != _value)
                        actionValueChange?.Invoke(_value = value);
                }
            }

            public ACurrency(int value) => _value = value;

            public abstract int Set(int value);
            public abstract int Add(int value);

            public abstract int Increment();
            public abstract int DecrementNotSignal();

            public void Signal() => actionValueChange?.Invoke(_value);

            public virtual void Dispose() { }
           
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
