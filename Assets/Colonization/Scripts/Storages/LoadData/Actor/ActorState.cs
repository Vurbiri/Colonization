//Assets\Colonization\Scripts\Storages\LoadData\Actor\ActorState.cs
namespace Vurbiri.Colonization.Storage
{
    public readonly struct ActorState
	{
        public readonly int id;
        public readonly int currentHP;
        public readonly int currentAP;
        public readonly int move;

        public ActorState(int id, int currentHP, int currentAP, int move)
        {
            this.id = id;
            this.currentHP = currentHP;
            this.currentAP = currentAP;
            this.move = move;
        }
    }
}
