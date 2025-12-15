namespace Vurbiri.Colonization
{
    public class ProfitSingle : IProfit
    {
        private readonly Id<CurrencyId> _currency;

        public IProfit Instance => this;
        public Id<CurrencyId> Currency => _currency;

        public ProfitSingle(IdFlags<ProfitId> profits) => _currency = profits.Convert<CurrencyId>().First();

        public Id<CurrencyId> Set() => _currency;
    }
}
