//Assets\Colonization\Scripts\Actors\ActorInitData.cs
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class ActorInitData
	{
        private readonly ITurn _turn;

        public readonly Id<PlayerId> owner;
        public readonly Diplomacy diplomacy;
        public readonly GameplayEventBus eventBus;
        public readonly IReactive<IPerk>[] buffs;

        public bool IsPlayerTurn => owner == PlayerId.Player & owner == _turn.CurrentId;

        public ActorInitData(Id<PlayerId> owner, Buffs artefact) : this(owner)
        {
            buffs = new IReactive<IPerk>[] { artefact };
        }

        public ActorInitData(DemonBuffs leveling, Buffs artefact) : this(PlayerId.Satan)
        {
            buffs = new IReactive<IPerk>[] { leveling, artefact };
        }

        private ActorInitData(Id<PlayerId> owner)
        {
            this.owner = owner;
            diplomacy = SceneServices.Get<Diplomacy>();
            eventBus = SceneServices.Get<GameplayEventBus>();
            _turn = SceneServices.Get<ITurn>();
        }
    }
}
