using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public class ProfitSingle : IProfit
    {
        private readonly Id<CurrencyId> _value;

        public IProfit Instance { [Impl(256)] get => this; }
        public Id<CurrencyId> Value { [Impl(256)] get => _value; }

        public ProfitSingle(IdFlags<CurrencyId> profits) => _value = profits.First();

        [Impl(256)] public Id<CurrencyId> Set() => _value;
    }
}
