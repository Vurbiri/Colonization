namespace Vurbiri.Colonization
{
    public struct TurnQueue
    {
        public Id<PlayerId> currentId;
        public int round;

        public readonly bool IsPerson => currentId == PlayerId.Person;
        public readonly bool IsNotPerson => currentId != PlayerId.Person;
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
