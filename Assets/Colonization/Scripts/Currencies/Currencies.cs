using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;
using Random = UnityEngine.Random;

namespace Vurbiri.Colonization
{
    [Serializable, JsonArray]
    public class Currencies : AReactive<int>, IEnumerable<int>
#if UNITY_EDITOR
        , ISerializationCallbackReceiver
#endif 
    {
        [SerializeField] private CurrencyMain[] _values = new CurrencyMain[CurrencyId.Count];
        [SerializeField] private int _amount = 0;
        private readonly int _countMain = CurrencyId.Count - 1;
        private readonly int _countAll = CurrencyId.Count;

        public int CountMain => _countMain;
        public int CountAll => _countAll;
        public int Amount
        {
            get => _amount;
            private set
            {
                if(_amount != value)
                    ActionValueChange?.Invoke(_amount = value);
            }
        }

        public int Blood => _values[CurrencyId.Blood];

        public int this[Id<CurrencyId> id] { get => _values[id.ToInt]; set => Set(id, value); }
        public int this[int index] { get => _values[index]; set => Set(index, value); }

        public Currencies(IReadOnlyList<int> array)
        {
            if (_countAll != array.Count)
                throw new ArgumentOutOfRangeException($"Currencies. Копирование из IReadOnlyList размера {array.Count} невозможно.");

            int value;
            for (int i = 0; i < _countMain; i++)
            {
                value = array[i];
                _values[i] = new CurrencyMain(value);
                _amount += value;
            }
            _values[CurrencyId.Blood] = new CurrencyBlood(array[CurrencyId.Blood]);
        }
        public Currencies(Currencies other)
        {
            for (int i = 0; i < _countMain; i++)
                _values[i] = new CurrencyMain(other._values[i]);
            _values[CurrencyId.Blood] = new CurrencyBlood(other._values[CurrencyId.Blood]);

            _amount = other._amount;
        }
        public Currencies()
        {
            for (int i = 0; i < _countMain; i++)
                _values[i] = new CurrencyMain();
            _values[CurrencyId.Blood] = new CurrencyBlood();
        }

        public Unsubscriber<int> Subscribe(int index, Action<int> action, bool calling = true) => _values[index].Subscribe(action, calling);
        public Unsubscriber<int> Subscribe(Id<CurrencyId> id, Action<int> action, bool calling = true) => _values[id.ToInt].Subscribe(action, calling);
        public void Unsubscribe(int index, Action<int> action) => _values[index].Unsubscribe(action);
        public void Unsubscribe(Id<CurrencyId> id, Action<int> action) => _values[id.ToInt].Unsubscribe(action);

        //TEST
        public void Rand(int max)
        {
            Debug.Log("TEST (Currencies)");
            for (int i = 0; i < _countAll; i++)
                Set(i, UnityEngine.Random.Range(0, max + 1));
        }

        public void Set(int index, int value) => Amount += _values[index].Set(value);
        public void Set(Id<CurrencyId> id, int value) => Amount += _values[id.ToInt].Set(value);

        public void SetFrom(Currencies other)
        {
            for (int i = 0; i < _countAll; i++)
                _values[i].Set(other._values[i]);

            _amount = other._amount;
        }

        public void Increment(int index) => Amount += _values[index].Increment();
        public void Increment(Id<CurrencyId> id) => Amount += _values[id.ToInt].Increment();

        public void Add(int index, int value)
        {
            if (value != 0)
                Amount += _values[index].Add(value);
        }
        public void Add(Id<CurrencyId> id, int value) => Add(id.ToInt, value);

        public void RandomMainAdd(int value)
        {
            if (value != 0)
                Amount += _values[Random.Range(0, _countMain)].Add(value);
        }

        public void AddFrom(Currencies other)
        {
            if (other._amount == 0)
                return;
            
            for (int i = 0; i < _countAll; i++)
                Amount += _values[Random.Range(0, _countMain)].Add(other._values[i]);
        }

        public void Pay(Currencies cost)
        {
            for (int i = 0; i < _countAll; i++)
                Amount += _values[Random.Range(0, _countMain)].Add(-cost._values[i]);
        }

        public void ClampMain(int max)
        {
            while(_amount > max)
                Amount += _values[Random.Range(0, _countMain)].Decrement();
        }

        public void Clear()
        {
            for (int i = 0; i < _countAll; i++)
                _values[i].Set(0);
            Amount = 0;
        }

        protected override void Callback(Action<int> action) => action(_amount);

        public static bool operator >(Currencies left, Currencies right) => !(left <= right);
        public static bool operator <(Currencies left, Currencies right) => !(left >= right);
        public static bool operator >=(Currencies left, Currencies right)
        {
            if (left == null || right == null || left._amount < right._amount)
                return false;

            for (int i = 0; i < left._countMain; i++)
                if (left._values[i] < right._values[i])
                    return false;
            return true;
        }
        public static bool operator <=(Currencies left, Currencies right)
        {
            if (left == null || right == null)
                return false;

            for (int i = 0; i < left._countMain; i++)
                if (left._values[i] > right._values[i])
                    return false;
            return true;
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < _countMain; i++)
                yield return _values[i];
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        #region Nested: CurrencyMain, CurrencyBlood
        //*******************************************************
        [Serializable]
        public class CurrencyMain : AReactive<int>
        {
            [SerializeField] protected int _value;

            protected int Value { set => ActionValueChange?.Invoke(_value = value); }

            public CurrencyMain() => _value = 0;
            public CurrencyMain(int value) => _value = value;

            protected override void Callback(Action<int> action) => action(_value);

            public virtual int Set(int value)
            {
                if(value == _value)
                    return 0;
                
                int delta = value - _value;
                Value = value;
                return delta;
            }
            public virtual int Add(int value)
            {
                Value = _value + value;
                return value;
            }
            public virtual int Increment()
            {
                Value = ++_value;
                return 1;
            }
            public virtual int Decrement()
            {
                if(_value == 0)
                    return 0;
                
                Value = --_value;
                return -1;
            }

            public static implicit operator int(CurrencyMain currency) => currency._value;
        }
        //*******************************************************
        [Serializable]
        public class CurrencyBlood : CurrencyMain
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
            public override int Increment()
            {
                Value = ++_value;
                return 0;
            }
            public override int Decrement()
            {
                if (_value > 0)
                    Value = --_value;
                return 0;
            }
        }

        #endregion

        #region ISerializationCallbackReceiver
#if UNITY_EDITOR
        public void OnBeforeSerialize()
        {
            if (Application.isPlaying)
                return;

            _values ??= new CurrencyMain[CurrencyId.Count];
            if (_values.Length != _countAll)
                Array.Resize(ref _values, _countAll);

            _amount = 0;
            CurrencyMain value;
            Type type = typeof(CurrencyMain);
            for (int i = 0; i < _countMain; i++)
            {
                _values[i] ??= new CurrencyMain();
                value = _values[i];
                if (value.GetType() != type)
                {
                    Debug.Log(value.GetType());
                    _values[i] = value = new CurrencyMain(value);
                }
                _amount += value;
            }

            _values[CurrencyId.Blood] ??= new CurrencyBlood();
            if (_values[CurrencyId.Blood].GetType() != typeof(CurrencyBlood))
            {
                Debug.Log(_values[CurrencyId.Blood].GetType());
                _values[CurrencyId.Blood] = new CurrencyBlood(_values[CurrencyId.Blood]);
            }
        }

        public void OnAfterDeserialize() { }
#endif
        #endregion
    }
}
