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

        public Event<Hexagon> EventHexagonSelect => _hexagonSelect;
        public Event<Crossroad> EventCrossroadSelect => _crossroadSelect;
        public Event<Actor> EventActorSelect => _actorSelect;
        public Event<Id<PlayerId>, Id<ActorTypeId>, int> EventActorKill => _actorKill;
        public Event<bool> EventUnselect => _unselect;

        public Event<bool> EventHexagonShow => _hexagonShow;
    }
}
