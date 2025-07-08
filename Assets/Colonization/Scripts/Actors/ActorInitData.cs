using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class ActorInitData
	{
        public readonly Id<PlayerId> owner;
        public readonly bool isPersonOwned;
        public readonly GameplayTriggerBus triggerBus;
        public readonly IReactive<IPerk>[] buffs;

        public ActorInitData(Id<PlayerId> owner, Artefact artefact, WarriorPerks perks) : this(owner)
        {
            buffs = new IReactive<IPerk>[] { artefact, perks };
        }

        public ActorInitData(DemonLeveling leveling, Artefact artefact) : this(PlayerId.Satan)
        {
            buffs = new IReactive<IPerk>[] { leveling, artefact };
        }

        private ActorInitData(Id<PlayerId> owner)
        {
            this.owner = owner;
            isPersonOwned = owner == PlayerId.Person;
            triggerBus = SceneContainer.Get<GameplayTriggerBus>();
        }
    }
}
