//Assets\Colonization\Scripts\Currencies\Profit\ProfitSingle.cs
namespace Vurbiri.Colonization
{
    public class ProfitSingle : IProfit
    {
        private readonly int _value;

        public ProfitSingle(int profit) => _value = profit;

        public int Get => _value;
    }
}
