namespace Vurbiri.Colonization
{
    public struct GameStates
    {
        public Balance balance;
        public Score score;
        public Diplomacy diplomacy;
        public Prices prices;

        public readonly void AddOrder(int playerId, int order)
        {
            if (order > 0)
            {
                balance.Add(order);
                score.ForAddingOrder(playerId, order);
            }
        }

        public void Clear()
        {
            balance = null; score = null; diplomacy = null; prices = null;
        }
    }
}
