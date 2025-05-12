//Assets\Colonization\Scripts\Currencies\Profit\ProfitArray.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class ProfitArray : IProfit
    {
        private readonly int[] _values;
        private int _value = -1;

        public int Value => _value;

        public ProfitArray(List<int> profits) => _values = profits.ToArray();

        public int Set() => _value = _values[Random.Range(0, _values.Length)];
    }
}
