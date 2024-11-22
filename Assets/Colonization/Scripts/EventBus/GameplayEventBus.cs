//Assets\Colonization\Scripts\EventBus\GameplayEventBus.cs
using System;

namespace Vurbiri.Colonization
{
    using Actors;

    public class GameplayEventBus
    {
        public event Action<Crossroad> EventCrossroadSelect;
        public void TriggerCrossroadSelect(Crossroad crossroad) => EventCrossroadSelect?.Invoke(crossroad);

        public event Action<Crossroad> EventCrossroadUnselect;
        public void TriggerCrossroadUnselect(Crossroad crossroad) => EventCrossroadUnselect?.Invoke(crossroad);
        
        public event Action<Actor> EventActorSelect;
        public void TriggerActorSelect(Actor actor) => EventActorSelect?.Invoke(actor);

        public event Action<Actor> EventActorUnselect;
        public void TriggerActorUnselect(Actor actor) => EventActorUnselect?.Invoke(actor);

        public event Action<Hexagon> EventHexagonSelect;
        public void TriggerHexagonSelect(Hexagon hex) => EventHexagonSelect?.Invoke(hex);

        public event Action<Hexagon> EventHexagonUnselect;
        public void TriggerHexagonUnselect(Hexagon hex) => EventHexagonUnselect?.Invoke(hex);

        public event Action<bool> EventCrossroadMarkShow;
        public void TriggerCrossroadMarkShow(bool show) => EventCrossroadMarkShow?.Invoke(show);

        public event Action<bool> EventHexagonIdShow;
        public void TriggerHexagonIdShow(bool show) => EventHexagonIdShow?.Invoke(show);

        public event Action EventSceneEndCreation;
        public void TriggerSceneEndCreation() => EventSceneEndCreation?.Invoke();
    }
}
