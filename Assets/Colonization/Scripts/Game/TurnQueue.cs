using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public struct TurnQueue
    {
        public Id<PlayerId> currentId;
        public int turn;

        public readonly bool IsPerson { [Impl(256)] get => currentId == PlayerId.Person; }
        public readonly bool IsNotPerson { [Impl(256)] get => currentId != PlayerId.Person; }
        public readonly bool IsSatan { [Impl(256)] get => currentId == PlayerId.Satan; }

        public TurnQueue(Id<PlayerId> current)
        {
            this.currentId = current;
            this.turn = 0;
        }

        public TurnQueue(int currentId, int round)
        {
            this.currentId = currentId;
            this.turn = round;
        }

        [Impl(256)]public void Next()
        {
            if (!currentId.Next())
            {
                currentId.Clamp();
                turn++;
            }
        }
    }
}
