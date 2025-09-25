using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public abstract class GameEventBus
    {
        protected readonly VAction<Hexagon> _hexagonSelect = new();
        protected readonly VAction<Crossroad> _crossroadSelect = new();
        protected readonly VAction<Actor> _actorSelect = new();
        protected readonly VAction<Id<PlayerId>, Id<ActorTypeId>, int> _actorKill = new();
        protected readonly VAction<bool> _unselect = new();

        protected readonly VAction<bool> _hexagonShow = new();

        public Event<Hexagon> EventHexagonSelect => _hexagonSelect;
        public Event<Crossroad> EventCrossroadSelect => _crossroadSelect;
        public Event<Actor> EventActorSelect => _actorSelect;
        public Event<Id<PlayerId>, Id<ActorTypeId>, int> EventActorKill => _actorKill;
        public Event<bool> EventUnselect => _unselect;

        public Event<bool> EventHexagonShow => _hexagonShow;
    }
}
