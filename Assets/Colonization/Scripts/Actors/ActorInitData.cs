using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class ActorInitData
	{
        public readonly Id<PlayerId> owner;
        public readonly IReactive<IPerk>[] buffs;

        public ActorInitData(Id<PlayerId> owner, WarriorPerks perks, Artefact artefact)
        {
            this.owner = owner;
            buffs = new IReactive<IPerk>[] { perks, artefact };
        }

        public ActorInitData(DemonLeveling leveling, Artefact artefact)
        {
            owner = PlayerId.Satan;
            buffs = new IReactive<IPerk>[] { leveling, artefact };
        }
    }
}
