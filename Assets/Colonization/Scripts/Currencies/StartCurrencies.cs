using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    sealed public class StartCurrencies
    {
        [SerializeField] private int[] _values = new int[CurrencyId.Count];
        [SerializeField] private int _amount = 0;
        [SerializeField] private int _blood = 0;

        public Currency this[int index] { [Impl(256)] get => new(_values[index]); }

        public int Amount { [Impl(256)] get => _amount; }
        public int BloodValue { [Impl(256)] get => _blood; }

        public StartCurrencies() { }
        public StartCurrencies(int[] array)
        {
            for (int i = 0; i < CurrencyId.Count; ++i)
            {
                _values[i] = array[i];
                _amount += array[i];
            }
            _blood = array[CurrencyId.Count];
        }
    }
}
