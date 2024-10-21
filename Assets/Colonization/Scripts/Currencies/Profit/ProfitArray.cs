using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public class ProfitArray : IProfit
    {
        private readonly int[] _values;

        public ProfitArray(IReadOnlyList<Id<CurrencyId>> profits)
        {
            int count = profits.Count;
            _values = new int[count];
            for(int i = 0; i < count; i++) 
                _values[i] = profits[i].Value;
        }

        public int Get => _values.Rand();

    }
}
