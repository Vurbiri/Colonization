using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    public class ActorInitData
	{
        public readonly Id<PlayerId> owner;
        public readonly ReadOnlyArray<IReactive<IPerk>> buffs;

        public ActorInitData(Id<PlayerId> owner, params IReactive<IPerk>[] buffs)
        {
            this.owner = owner;
            this.buffs = new(buffs);
        }
    }
}
