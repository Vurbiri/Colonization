//Assets\Colonization\Scripts\EventBus\GameplayTriggerBus.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
    public class GameplayTriggerBus : GameplayEventBus
    {
        public void TriggerCrossroadSelect(Crossroad crossroad) => _crossroadSelect.Invoke(crossroad);
        public void TriggerActorSelect(Actor actor) => _actorSelect.Invoke(actor);
        public void TriggerUnselect(bool isEquals) => _unselect.Invoke(isEquals);

        public void TriggerActorKilling(Id<PlayerId> self, Id<PlayerId> target, int actorId) => _actorKilling.Invoke(self, target, actorId);

        public void TriggerHexagonIdShow(bool show) => _hexagonIdShow.Invoke(show);
    }
}
