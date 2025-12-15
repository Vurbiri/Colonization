using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public class ProfitArray : IProfit
    {
        private static ReadOnlyArray<Id<CurrencyId>> s_values;
        private Id<CurrencyId> _currency = CurrencyId.None;

        public IProfit Instance => new ProfitArray();
        public Id<CurrencyId> Currency => _currency;

        public ProfitArray(IdFlags<ProfitId> profits) => s_values ??= new(profits.Convert<CurrencyId>().GetValues().ToArray());
        private ProfitArray() { }

        public Id<CurrencyId> Set() => _currency = s_values.Rand();
    }
}
