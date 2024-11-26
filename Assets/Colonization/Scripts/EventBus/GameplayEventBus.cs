//Assets\Colonization\Scripts\EventBus\GameplayEventBus.cs
using System;

namespace Vurbiri.Colonization
{
    using Actors;

    public class GameplayEventBus
    {
        public event Action<Crossroad> EventCrossroadSelect;
        public void TriggerCrossroadSelect(Crossroad crossroad) => EventCrossroadSelect?.Invoke(crossroad);

        public event Action<Actor> EventActorSelect;
        public void TriggerActorSelect(Actor actor) => EventActorSelect?.Invoke(actor);
        
        public event Action EventUnselect;
        public void TriggerUnselect() => EventUnselect?.Invoke();

        public event Action<Id<PlayerId>, Id<PlayerId>> EventStartTurn;
        public void TriggerStartTurn(Id<PlayerId> prev, Id<PlayerId> current) => EventStartTurn?.Invoke(prev, current);


        public event Action<bool> EventCrossroadMarkShow;
        public void TriggerCrossroadMarkShow(bool show) => EventCrossroadMarkShow?.Invoke(show);

        public event Action<bool> EventHexagonIdShow;
        public void TriggerHexagonIdShow(bool show) => EventHexagonIdShow?.Invoke(show);

        public event Action EventSceneEndCreation;
        public void TriggerSceneEndCreation() => EventSceneEndCreation?.Invoke();
    }
}
