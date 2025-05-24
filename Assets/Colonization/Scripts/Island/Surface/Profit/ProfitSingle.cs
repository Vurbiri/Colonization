namespace Vurbiri.Colonization
{
    public class ProfitSingle : IProfit
    {
        private readonly int _value;

        public int Value => _value;

        public ProfitSingle(int profit) => _value = profit;

        public int Set() => _value;
    }
}
