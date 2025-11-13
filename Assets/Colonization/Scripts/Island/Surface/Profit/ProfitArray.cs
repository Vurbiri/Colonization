using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public class ProfitArray : IProfit
    {
        private static ReadOnlyArray<Id<CurrencyId>> s_values;
        private int _value = -1;

        public IProfit Instance { [Impl(256)] get => new ProfitArray(); }
        public Id<CurrencyId> Value { [Impl(256)] get => _value; }

        public ProfitArray(IdFlags<CurrencyId> profits) => s_values ??= new(profits.GetValues());
        private ProfitArray() { }

        [Impl(256)] public Id<CurrencyId> Set() => _value = s_values.Rand();
    }
}
