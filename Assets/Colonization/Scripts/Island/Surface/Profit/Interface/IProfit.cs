namespace Vurbiri.Colonization
{
    public interface IProfit
    {
        public int Value { get; }
        public IProfit Instance { get; }

        public int Set();
    }
}
