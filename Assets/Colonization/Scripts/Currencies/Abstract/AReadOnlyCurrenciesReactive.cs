using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public abstract class AReadOnlyCurrenciesReactive : ACurrencies, IReactiveSubValues<int, CurrencyId>
    {
        protected ACurrency[] _values = new ACurrency[countAll];
        protected Action<int> ActionAmountChange;

        public override int Amount
        {
            get => _amount;
            protected set { if (_amount != value) ActionAmountChange?.Invoke(_amount = value); }
        }

        public override int this[int index] { get => _values[index].Value; }
        public override int this[Id<CurrencyId> id] { get => _values[id.Value].Value; }

        public AReadOnlyCurrenciesReactive(IReadOnlyList<int> array)
        {
            if (countAll != array.Count)
                throw new ArgumentOutOfRangeException($"Currencies. Копирование из IReadOnlyList размера {array.Count} невозможно.");

            int value;
            for (int i = 0; i < countMain; i++)
            {
                value = array[i];
                _values[i] = new CurrencyMain(value);
                _amount += value;
            }
            _values[CurrencyId.Blood] = new CurrencyBlood(array[CurrencyId.Blood]);
        }
        public AReadOnlyCurrenciesReactive(ACurrencies other)
        {
            for (int i = 0; i < countMain; i++)
                _values[i] = new CurrencyMain(other[i]);
            _values[CurrencyId.Blood] = new CurrencyBlood(other[CurrencyId.Blood]);

            _amount = other.Amount;
        }
        public AReadOnlyCurrenciesReactive()
        {
            for (int i = 0; i < countMain; i++)
                _values[i] = new CurrencyMain();
            _values[CurrencyId.Blood] = new CurrencyBlood();
        }

        #region Reactive
        public Unsubscriber<int> Subscribe(Action<int> action, bool calling = true)
        {
            ActionAmountChange -= action;
            ActionAmountChange += action;
            if (calling && action != null)
                action(_amount);

            return new(this, action);
        }
        public Unsubscriber<int> Subscribe(int index, Action<int> action, bool calling = true) => _values[index].Subscribe(action, calling);
        public Unsubscriber<int> Subscribe(Id<CurrencyId> id, Action<int> action, bool calling = true) => _values[id.Value].Subscribe(action, calling);
        public void Unsubscribe(Action<int> action) => ActionAmountChange -= action;
        public void Unsubscribe(int index, Action<int> action) => _values[index].Unsubscribe(action);
        public void Unsubscribe(Id<CurrencyId> id, Action<int> action) => _values[id.Value].Unsubscribe(action);
        #endregion

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
            public override int AddAndClamp(int value, int max) => Set(Mathf.Clamp(_value + value, 0, max));

            public override int Increment()
            {
                Value = ++_value;
                return 1;
            }
            public override int DecrementNotMessage()
            {
                if (_value == 0)
                    return 0;

                _value = --_value;
                return -1;
            }
        }
        //*******************************************************
        protected class CurrencyBlood : ACurrency
        {
            public CurrencyBlood() : base(0) { }
            public CurrencyBlood(int value) : base(value) { }

            public override int Set(int value)
            {
                if (value != _value)
                    Value = value;
                return 0;
            }
            public override int Add(int value)
            {
                Value = _value + value;
                return 0;
            }
            public override int AddAndClamp(int value, int max) => Set(Mathf.Clamp(_value + value, 0, max));
            
            public override int Increment()
            {
                Value = ++_value;
                return 0;
            }
            
            public override int DecrementNotMessage()
            {
                if (_value > 0)
                    _value = --_value;
                return 0;
            }
        }
        //*******************************************************
        protected abstract class ACurrency : AReactive<int>
        {
            protected int _value;

            public override int Value { get => _value; protected set => ActionValueChange?.Invoke(_value = value); }

            public ACurrency(int value) => _value = value;

            public abstract int Set(int value);
            public abstract int Add(int value);
            public abstract int AddAndClamp(int value, int max);

            public abstract int Increment();
            public abstract int DecrementNotMessage();

            public void SendMessage() => ActionValueChange?.Invoke(_value);

            public static explicit operator int(ACurrency currency) => currency._value;
        }
        #endregion
     }
}
