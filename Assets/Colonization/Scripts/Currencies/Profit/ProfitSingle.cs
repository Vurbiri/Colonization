//Assets\Colonization\Scripts\Currencies\Profit\ProfitSingle.cs
namespace Vurbiri.Colonization
{
    public class ProfitSingle : IProfit
    {
        private readonly int _value;

        public ProfitSingle(Id<CurrencyId> profit) => _value = profit.Value;

        public int Get => _value;
    }
}
