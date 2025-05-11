//Assets\Colonization\Scripts\Actors\ActorInitData.cs
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class ActorInitData
	{
        private readonly TurnQueue _turn;

        public readonly Id<PlayerId> owner;
        public readonly bool isPlayerOwned;
        public readonly Diplomacy diplomacy;
        public readonly GameplayTriggerBus triggerBus;
        public readonly IReactive<IPerk>[] buffs;

        public bool IsPlayerTurn => owner == _turn.CurrentId & isPlayerOwned;

        public ActorInitData(Id<PlayerId> owner, Buffs artefact, WarriorPerks perks, TurnQueue turn) : this(owner, turn)
        {
            buffs = new IReactive<IPerk>[] { artefact, perks };
        }

        public ActorInitData(DemonBuffs leveling, Buffs artefact, TurnQueue turn) : this(PlayerId.Satan, turn)
        {
            buffs = new IReactive<IPerk>[] { leveling, artefact };
        }

        private ActorInitData(Id<PlayerId> owner, TurnQueue turn)
        {
            this.owner = owner;
            isPlayerOwned = owner == PlayerId.Player;
            diplomacy = SceneContainer.Get<Diplomacy>();
            triggerBus = SceneContainer.Get<GameplayTriggerBus>();
            _turn = turn;
        }
    }
}
