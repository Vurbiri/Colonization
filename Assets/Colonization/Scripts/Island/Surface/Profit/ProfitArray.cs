using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public class ProfitArray : IProfit
    {
        private static Vurbiri.Collections.ReadOnlyArray<int> s_values;
        private int _value = -1;

        public IProfit Instance { [Impl(256)] get => new ProfitArray(); }
        public int Value { [Impl(256)] get => _value; }

        public ProfitArray(IdFlags<CurrencyId> profits) => s_values ??= profits.GetValues().ToArray();
        private ProfitArray() { }

        [Impl(256)] public int Set() => _value = s_values.Rand();
    }
}
