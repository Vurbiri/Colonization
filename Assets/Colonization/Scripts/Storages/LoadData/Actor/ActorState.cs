namespace Vurbiri.Colonization.Storage
{
    public readonly struct ActorState
	{
        public readonly int id;
        public readonly int currentHP;
        public readonly int currentAP;
        public readonly int move;
        public readonly bool zealCharge;

        public ActorState(int id, int currentHP, int currentAP, int move, bool zealCharge)
        {
            this.id = id;
            this.currentHP = currentHP;
            this.currentAP = currentAP;
            this.move = move;
            this.zealCharge = zealCharge;
        }
    }
}
