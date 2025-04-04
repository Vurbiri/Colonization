//Assets\Colonization\Scripts\Currencies\Profit\ProfitArray.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class ProfitArray : IProfit
    {
        private readonly int[] _values;
        private readonly int _count;

        public ProfitArray(List<int> profits)
        {
            _values = profits.ToArray();
            _count = _values.Length;
        }

        public int Get => _values[Random.Range(0, _count)];

    }
}
