namespace Vurbiri.Colonization
{
    public interface IProfit
    {
        public Id<CurrencyId> Value { get; }
        public IProfit Instance { get; }

        public Id<CurrencyId> Set();
    }
}
