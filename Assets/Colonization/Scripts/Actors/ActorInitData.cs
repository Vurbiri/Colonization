//Assets\Colonization\Scripts\Actors\ActorInitData.cs
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class ActorInitData
	{
        private readonly TurnQueue _turn;

        public readonly Id<PlayerId> owner;
        public readonly Diplomacy diplomacy;
        public readonly GameplayTriggerBus triggerBus;
        public readonly IReactive<IPerk>[] buffs;

        public bool IsPlayerTurn => owner == PlayerId.Player & owner == _turn.CurrentId;

        public ActorInitData(Id<PlayerId> owner, Buffs artefact, WarriorPerks perks) : this(owner)
        {
            buffs = new IReactive<IPerk>[] { artefact, perks };
        }

        public ActorInitData(DemonBuffs leveling, Buffs artefact) : this(PlayerId.Satan)
        {
            buffs = new IReactive<IPerk>[] { leveling, artefact };
        }

        private ActorInitData(Id<PlayerId> owner)
        {
            this.owner = owner;
            diplomacy = SceneContainer.Get<Diplomacy>();
            triggerBus = SceneContainer.Get<GameplayTriggerBus>();
            _turn = SceneContainer.Get<TurnQueue>();
        }
    }
}
