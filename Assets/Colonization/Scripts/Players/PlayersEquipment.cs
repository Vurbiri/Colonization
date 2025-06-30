using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
	public class PlayersEquipment
	{
        public readonly Prices prices;
        public readonly Roads[] roads = new Roads[PlayerId.HumansCount];
        public readonly ReadOnlyReactiveSet<Actor>[] actors = new ReactiveSet<Actor>[PlayerId.Count];
        public readonly IdArray<EdificeGroupId, ReadOnlyReactiveList<Crossroad>>[] edifices = new IdArray<EdificeGroupId, ReadOnlyReactiveList<Crossroad>>[PlayerId.HumansCount];
               

        public PlayersEquipment(Prices prices)
        {
            this.prices = prices;
        }

        public void Add(Human human)
        {
            int id = human.Id.Value;

            roads[id] = human.Roads;
            actors[id] = human.Warriors;

            var humanEdifices = edifices[id] = new();
            for (int i = 0; i < EdificeGroupId.Count; i++)
                humanEdifices[i] = human.GetEdifices(i);
        }

        public void Add(Satan satan)
        {
            actors[PlayerId.Satan] = satan.Demons;
        }
    }
}
