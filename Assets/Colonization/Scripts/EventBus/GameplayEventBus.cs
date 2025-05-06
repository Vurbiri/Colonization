//Assets\Colonization\Scripts\EventBus\GameplayEventBus.cs
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public abstract class GameplayEventBus
    {
        protected readonly Signer<Crossroad> _crossroadSelect = new();
        protected readonly Signer<Actor> _actorSelect = new();
        protected readonly Signer<bool> _unselect = new();

        protected readonly Signer<Id<PlayerId>, Id<PlayerId>, int> _actorKilling = new();

        protected readonly Signer<bool> _hexagonIdShow = new();

        public ISigner<Crossroad> EventCrossroadSelect => _crossroadSelect;
        public ISigner<Actor> EventActorSelect => _actorSelect;
        public ISigner<bool> EventUnselect => _unselect;

        public ISigner<Id<PlayerId>, Id<PlayerId>, int> EventActorKilling => _actorKilling;

        public ISigner<bool> EventHexagonIdShow => _hexagonIdShow;

    }
}
