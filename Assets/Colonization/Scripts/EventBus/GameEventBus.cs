using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public abstract class GameEventBus
    {
        protected readonly Subscription<Hexagon> _hexagonSelect = new();
        protected readonly Subscription<Crossroad> _crossroadSelect = new();
        protected readonly Subscription<Actor> _actorSelect = new();
        protected readonly Subscription<Id<PlayerId>, Id<ActorTypeId>, int> _actorKill = new();
        protected readonly Subscription<bool> _unselect = new();

        protected readonly Subscription<bool> _hexagonShow = new();

        public ISubscription<Hexagon> EventHexagonSelect => _hexagonSelect;
        public ISubscription<Crossroad> EventCrossroadSelect => _crossroadSelect;
        public ISubscription<Actor> EventActorSelect => _actorSelect;
        public ISubscription<Id<PlayerId>, Id<ActorTypeId>, int> EventActorKill => _actorKill;
        public ISubscription<bool> EventUnselect => _unselect;

        public ISubscription<bool> EventHexagonShow => _hexagonShow;
    }
}
