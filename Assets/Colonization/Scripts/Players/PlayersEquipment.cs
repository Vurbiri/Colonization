using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
	public class PlayersEquipment
	{
        public readonly ReadOnlyReactiveSet<Actor>[] actors = new ReactiveSet<Actor>[PlayerId.Count];

        public readonly Roads[] roads = new Roads[PlayerId.HumansCount];
    }
}
