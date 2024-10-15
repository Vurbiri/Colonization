namespace Vurbiri.Colonization
{
    public class ProfitSingle : IProfit
    {
        private readonly int _value;

        public ProfitSingle(Id<CurrencyId> profit) => _value = profit.ToInt;

        public int Get => _value;
    }
}
