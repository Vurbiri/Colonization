namespace Vurbiri.Colonization
{
    public struct TurnQueue
    {
        public Id<PlayerId> currentId;
        public int round;

        public readonly bool IsPlayer => currentId == PlayerId.Player;
        public readonly bool IsNotPlayer => currentId != PlayerId.Player;
        public readonly bool IsSatan => currentId == PlayerId.Satan;

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
