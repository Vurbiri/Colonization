using System;

namespace Vurbiri.Colonization
{
    public struct GameStates : IDisposable
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

        public void Dispose()
        {
            balance = null; score = null; diplomacy = null;
        }
    }
}
