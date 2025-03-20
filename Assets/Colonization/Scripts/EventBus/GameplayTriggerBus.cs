//Assets\Colonization\Scripts\EventBus\GameplayTriggerBus.cs
using System;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
    public class GameplayTriggerBus : GameplayEventBus, IDisposable
    {
        public void TriggerCrossroadSelect(Crossroad crossroad) => _crossroadSelect.Invoke(crossroad);
        public void TriggerActorSelect(Actor actor) => _actorSelect.Invoke(actor);
        public void TriggerUnselect() => _unselect.Invoke();
        
        public void TriggerHexagonIdShow(bool show) => _hexagonIdShow.Invoke(show);

        public void Dispose()
        {
            _crossroadSelect.Dispose();
            _actorSelect.Dispose();
            _unselect.Dispose();
            _hexagonIdShow.Dispose();
        }
    }
}
