namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        sealed protected class IdleState : AState
        {
            public IdleState(Actor parent) : base(parent) { }

            public override void Enter()
            {
                _skin.Idle();
                ActorInteractable = _actor._canUseSkills = true;
            }

            public override void Exit()
            {
                ActorInteractable = _actor._canUseSkills = false;
            }

            public override void Select() => GameContainer.TriggerBus.TriggerActorSelect(_actor);
            public override void Unselect(ISelectable newSelectable) => GameContainer.TriggerBus.TriggerUnselect(_actor.Equals(newSelectable));
        }
    }
}
