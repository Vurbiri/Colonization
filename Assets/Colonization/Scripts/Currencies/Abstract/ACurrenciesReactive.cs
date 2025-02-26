//Assets\Colonization\Scripts\Currencies\Abstract\ACurrenciesReactive.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public abstract class ACurrenciesReactive : ACurrencies, IDisposable, IReactive<int, int>
    {
        protected ACurrency[] _values = new ACurrency[countAll];
        protected ReactiveValue<int> _amount = new(0);
        protected IReadOnlyReactive<int> _maxValueMain, _maxValueBlood;
        private readonly Subscriber<int, int> _subscriber = new();

        public override int Amount => _amount.Value;
        public IReactive<int> AmountReactive => _amount;
        public IReactive<int> AmountMax => _maxValueMain;
        public IReactive<int> BloodCurrent => _values[CurrencyId.Blood];
        public IReactive<int> BloodMax => _maxValueBlood;

        public override int this[int index] { get => _values[index].Value; }
        public override int this[Id<CurrencyId> id] { get => _values[id.Value].Value; }

        #region Constructions
        public ACurrenciesReactive(IReadOnlyList<int> array, IReadOnlyReactive<int> maxValueMain, IReadOnlyReactive<int> maxValueBlood)
        {
            _maxValueMain = maxValueMain;
            _maxValueBlood = maxValueBlood;

            int value, amount = 0;
            for (int i = 0; i < countMain; i++)
            {
                int index = i;
                value = array[i];
                _values[i] = new CurrencyMain(value);
                _values[i].Subscribe(v => _subscriber.Invoke(index, v), false);
                amount += value;
            }
            _values[CurrencyId.Blood] = new CurrencyBlood(array[CurrencyId.Blood], maxValueBlood);
            _values[CurrencyId.Blood].Subscribe(v => _subscriber.Invoke(CurrencyId.Blood, v), false);

            _amount.SilentValue = amount;
        }
        public ACurrenciesReactive(ACurrencies other, IReadOnlyReactive<int> maxValueMain, IReadOnlyReactive<int> maxValueBlood)
        {
            _maxValueMain = maxValueMain;
            _maxValueBlood = maxValueBlood;

            for (int i = 0; i < countMain; i++)
            {
                _values[i] = new CurrencyMain(other[i]);
                int index = i;
                _values[i].Subscribe(v => _subscriber.Invoke(index, v), false);
            }
            _values[CurrencyId.Blood] = new CurrencyBlood(other[CurrencyId.Blood], maxValueBlood);
            _values[CurrencyId.Blood].Subscribe(v => _subscriber.Invoke(CurrencyId.Blood, v), false);

            _amount.SilentValue = other.Amount;
        }

        public ACurrenciesReactive(IReadOnlyReactive<int> maxValueMain, IReadOnlyReactive<int> maxValueBlood)
        {
            _maxValueMain = maxValueMain;
            _maxValueBlood = maxValueBlood;

            for (int i = 0; i < countMain; i++)
            {
                _values[i] = new CurrencyMain();
                int index = i;
                _values[i].Subscribe(v => _subscriber.Invoke(index, v), false);
            }
            _values[CurrencyId.Blood] = new CurrencyBlood(maxValueBlood);
            _values[CurrencyId.Blood].Subscribe(v => _subscriber.Invoke(CurrencyId.Blood, v), false);
        }

        public ACurrenciesReactive()
        {
            for (int i = 0; i < countMain; i++)
            {
                _values[i] = new CurrencyMain();
                int index = i;
                _values[i].Subscribe(v => _subscriber.Invoke(index, v), false);
            }
            _values[CurrencyId.Blood] = new CurrencyBlood();
            _values[CurrencyId.Blood].Subscribe(v => _subscriber.Invoke(CurrencyId.Blood, v), false);
        }
        #endregion

        #region Reactive
        public Unsubscriber Subscribe(Action<int, int> action, bool calling = true)
        {
            if (calling)
            {
                for (int i = 0; i < countAll; i++)
                    action(i, _values[i].Value);
            }

            return _subscriber.Add(action);
        }
        public Unsubscriber Subscribe(int index, Action<int> action, bool calling = true) => _values[index].Subscribe(action, calling);
        public Unsubscriber Subscribe(Id<CurrencyId> id, Action<int> action, bool calling = true) => _values[id.Value].Subscribe(action, calling);
        #endregion

        public void Dispose()
        {
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
            private readonly Unsubscriber _unsubscriber;

            public override int Value 
            { 
                get => _value; 
                protected set
                {
                    if (value != _value)
                        base._subscriber.Invoke(_value = Mathf.Clamp(value, 0, _max));
                }
            }

            public CurrencyBlood() : base(0) { }
            public CurrencyBlood(IReactive<int> maxValue) : this(0, maxValue) { }
            public CurrencyBlood(int value, IReactive<int> maxValue) : base(value)
            {
                _unsubscriber = maxValue.Subscribe(v => _max = v);
            }

            public override int Set(int value)
            {
                if (value != _value)
                    base._subscriber.Invoke(_value = Mathf.Clamp(value, 0, _max));
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
                _unsubscriber?.Unsubscribe();
                _subscriber.Dispose();
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
                        _subscriber.Invoke(_value = value);
                }
            }

            public ACurrency(int value) => _value = value;

            public abstract int Set(int value);
            public abstract int Add(int value);

            public abstract int Increment();
            public abstract int DecrementNotSignal();

            public void Signal() => _subscriber.Invoke(_value);

           
            public static explicit operator int(ACurrency currency) => currency._value;

            public bool Equals(ACurrency other) => other is not null && _value == other._value;
            public int CompareTo(ACurrency other) => -_value.CompareTo(other._value);
            public override bool Equals(object other) => Equals(other as ACurrency);
            public override int GetHashCode() => _value.GetHashCode();
            public override string ToString() => _value.ToString();
            public virtual void Dispose() { }

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
