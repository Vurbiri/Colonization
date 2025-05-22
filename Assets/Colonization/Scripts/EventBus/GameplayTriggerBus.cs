//Assets\Colonization\Scripts\EventBus\GameplayTriggerBus.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
    public class GameplayTriggerBus : GameplayEventBus
    {
        public void TriggerCrossroadSelect(Crossroad crossroad) => _crossroadSelect.Invoke(crossroad);
        public void TriggerActorSelect(Actor actor) => _actorSelect.Invoke(actor);
        public void TriggerUnselect(bool isEquals) => _unselect.Invoke(isEquals);

        public void TriggerHexagonShowDistance(bool show) => _hexagonShowDistance.Invoke(show);
        public void TriggerHexagonShow(bool show) => _hexagonShow.Invoke(show);
    }
}
