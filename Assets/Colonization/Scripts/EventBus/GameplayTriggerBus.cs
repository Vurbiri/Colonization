using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
    public class GameplayTriggerBus : GameplayEventBus
    {
        public void TriggerCrossroadSelect(Crossroad crossroad) => _crossroadSelect.Invoke(crossroad);
        public void TriggerActorSelect(Actor actor) => _actorSelect.Invoke(actor);
        public void TriggerActorKill(Id<PlayerId> killer, Id<ActorTypeId> deadType, int deadId) => _actorKill.Invoke(killer, deadType, deadId);
        public void TriggerUnselect(bool isEquals) => _unselect.Invoke(isEquals);

        public void TriggerHexagonShowDistance(bool show) => _hexagonShowDistance.Invoke(show);
        public void TriggerHexagonShow(bool show) => _hexagonShow.Invoke(show);
    }
}
