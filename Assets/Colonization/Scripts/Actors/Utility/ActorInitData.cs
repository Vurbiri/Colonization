using Vurbiri.Collections;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
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
