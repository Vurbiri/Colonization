using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    using static CurrencyId;

    [System.Serializable]
    sealed public class CurrenciesLite : ACurrencies
    {
        [SerializeField] private int[] _values = new int[AllCount];
        [SerializeField] private int _amount = 0;

        public override int Amount { [Impl(256)] get => _amount; }
        public override bool IsEmpty { [Impl(256)] get => _amount == 0 & _values[Blood] == 0; }

        public override int this[int index] { [Impl(256)] get => _values[index];}
        public override int this[Id<CurrencyId> id] { [Impl(256)] get => _values[id.Value]; }

        public CurrenciesLite() { }
        public CurrenciesLite(int[] array)
        {
            for (int i = 0; i < MainCount; ++i)
            {
                _values[i] = array[i];
                _amount += array[i];
            }
            _values[Blood] = array[Blood];
        }

        public override IEnumerator<int> GetEnumerator() => new ArrayEnumerator<int>(_values, AllCount);
    }
}
