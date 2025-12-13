using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public struct TurnQueue
    {
        public Id<PlayerId> currentId;
        public bool isPerson;
        public int turn;

        public TurnQueue(Id<PlayerId> current) : this(current, 0) { }
        public TurnQueue(int currentId, int round)
        {
            this.currentId = currentId;
            this.turn = round;
            isPerson = false;
        }

        [Impl(256)] public void SetPerson() => isPerson = GameContainer.Players[currentId].IsPerson;

        [Impl(256)] public void Next()
        {
            if (!currentId.Next())
            {
                currentId.Clamp();
                turn++;
            }
            SetPerson();
        }

        [Impl(256)] public void Reset()
        {
            currentId = PlayerId.None;
            isPerson = false;
        }
    }
}
