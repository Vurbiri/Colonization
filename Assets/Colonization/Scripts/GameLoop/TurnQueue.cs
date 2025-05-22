//Assets\Colonization\Scripts\GameLoop\TurnQueue.cs
namespace Vurbiri.Colonization
{
    public struct TurnQueue
    {
        public Id<PlayerId> currentId;
        public int round;

        public readonly bool IsCurrentPlayer => currentId == PlayerId.Player;

        public TurnQueue(int current)
        {
            this.currentId = current;
            this.round = 0;
        }

        public TurnQueue(int currentId, int round)
        {
            this.currentId = currentId;
            this.round = round;
        }

        public void Next()
        {
            if (currentId.Next() == 0) 
                round++;
        }
    }
}
