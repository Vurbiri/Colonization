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
                _actor.Interactable = true;
            }

            public override void Exit()
            {
                _actor.Interactable = false;
            }

            public override void Select() => s_triggerBus.TriggerActorSelect(_actor);
            public override void Unselect(ISelectable newSelectable)
            {
                s_triggerBus.TriggerUnselect(_actor.Equals(newSelectable));
            }
        }
    }
}
