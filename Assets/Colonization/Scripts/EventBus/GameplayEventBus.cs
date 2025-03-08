//Assets\Colonization\Scripts\EventBus\GameplayEventBus.cs
using System;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{

    public class GameplayEventBus
    {
        #region Selectable
        public event Action<Crossroad> EventCrossroadSelect;
        public void TriggerCrossroadSelect(Crossroad crossroad) => EventCrossroadSelect?.Invoke(crossroad);

        public event Action<Actor> EventActorSelect;
        public void TriggerActorSelect(Actor actor) => EventActorSelect?.Invoke(actor);
        
        public event Action EventUnselect;
        public void TriggerUnselect() => EventUnselect?.Invoke();
        #endregion

        #region UI
        public event Action<bool> EventHexagonIdShow;
        public void TriggerHexagonIdShow(bool show) => EventHexagonIdShow?.Invoke(show);
        #endregion

        #region GameLoop
        public event Action EventSceneEndCreation;
        public void TriggerSceneEndCreation() => EventSceneEndCreation?.Invoke();

        #endregion
    }
}
