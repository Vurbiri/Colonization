//Assets\Colonization\Scripts\EventBus\GameplayEventBus.cs
using System;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public abstract class GameplayEventBus
    {
        protected readonly Signer<Crossroad> _crossroadSelect = new();
        protected readonly Signer<Actor> _actorSelect = new();
        protected readonly Signer _unselect = new();

        protected readonly Signer<Id<PlayerId>, Id<PlayerId>, int> _actorKilling = new();

        protected readonly Signer<bool> _hexagonIdShow = new();

        public ISigner<Crossroad> EventCrossroadSelect => _crossroadSelect;
        public ISigner<Actor> EventActorSelect => _actorSelect;
        public ISigner EventUnselect => _unselect;

        public ISigner<Id<PlayerId>, Id<PlayerId>, int> EventActorKilling => _actorKilling;

        public ISigner<bool> EventHexagonIdShow => _hexagonIdShow;


        #region GameLoop
        public event Action EventSceneEndCreation;
        public void TriggerSceneEndCreation() => EventSceneEndCreation?.Invoke();
        #endregion
    }
}
