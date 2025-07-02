using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
	public partial class SpellBook
	{
        private readonly SpellBookSettings _settings;

        private readonly Coroutines _coroutines;
        private readonly CameraController _cameraController;

        private readonly Human[] _humans = new Human[PlayerId.HumansCount];

        private readonly Roads[] _roads = new Roads[PlayerId.HumansCount];
        private readonly ReadOnlyReactiveSet<Actor>[] _actors = new ReactiveSet<Actor>[PlayerId.Count];
        private readonly IdArray<EdificeGroupId, ReadOnlyReactiveList<Crossroad>>[] _edifices = new IdArray<EdificeGroupId, ReadOnlyReactiveList<Crossroad>>[PlayerId.HumansCount];

        // Economic 0
        //public void Order(int playerId, int mana)
        //{
        //    _humans[playerId].BuyOrder(mana * _settings.orderPerMana, mana);
        //}
    }
}
