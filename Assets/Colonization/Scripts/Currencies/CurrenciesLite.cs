using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Vurbiri.Colonization
{
    [Serializable]
    public class CurrenciesLite : ACurrencies
#if UNITY_EDITOR
        ,ISerializationCallbackReceiver
#endif 
    {
        [SerializeField] protected int[] _values = new int[CurrencyId.CountAll];
        [SerializeField] protected int _amount = 0;

        public override int Amount { get => _amount;}

        public override int this[int index] { get => _values[index]; }
        public override int this[Id<CurrencyId> id] { get => _values[id.Value]; }

        public static CurrenciesLite Empty => _empty;
        private static readonly CurrenciesLite _empty = new();

        public void Increment(int index)
        {
            _values[index]++;
            _amount++;
        }

        public void Set(int index, int value)
        {
            int temp = _values[index];
            _values[index] = value;
            _amount += value - temp;
        }

        public void Add(int index, int value)
        {
            _values[index] += value;
            _amount += value;
        }

        public void Multiply(int ratio)
        {
            if (_amount == 0)
                return;

            for (int i = 0; i < countMain; i++)
                _values[i] *= ratio;

            _amount *= ratio;
        }

        public void RandomMainAdd(int value)
        {
            _values[Random.Range(0, countMain)] += value;
            _amount += value;
        }

        #region ISerializationCallbackReceiver
#if UNITY_EDITOR
        public void OnBeforeSerialize()
        {
            if (Application.isPlaying)
                return;

            _values ??= new int[CurrencyId.Count];
            if (_values.Length != countAll)
                Array.Resize(ref _values, countAll);

            _amount = 0;
            for (int i = 0; i < countMain; i++)
                _amount += _values[i];

        }

        public void OnAfterDeserialize() { }
#endif
        #endregion
    }
}
