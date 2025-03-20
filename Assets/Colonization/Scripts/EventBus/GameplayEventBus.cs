//Assets\Colonization\Scripts\EventBus\GameplayEventBus.cs
using System;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public abstract class GameplayEventBus
    {
        protected readonly Subscriber<Crossroad> _crossroadSelect = new();
        protected readonly Subscriber<Actor> _actorSelect = new();
        protected readonly Subscriber _unselect = new();

        protected readonly Subscriber<bool> _hexagonIdShow = new();

        public ISubscriber<Crossroad> EventCrossroadSelect => _crossroadSelect;
        public ISubscriber<Actor> EventActorSelect => _actorSelect;
        public ISubscriber EventUnselect => _unselect;

        public ISubscriber<bool> EventHexagonIdShow => _hexagonIdShow;


        #region GameLoop
        public event Action EventSceneEndCreation;
        public void TriggerSceneEndCreation() => EventSceneEndCreation?.Invoke();
        #endregion
    }
}
