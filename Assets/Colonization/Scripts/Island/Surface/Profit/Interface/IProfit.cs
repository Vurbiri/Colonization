namespace Vurbiri.Colonization
{
    public interface IProfit
    {
        public Id<CurrencyId> Currency { get; }
        public IProfit Instance { get; }

        public Id<CurrencyId> Set();
    }
}
