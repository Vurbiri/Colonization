namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        public abstract partial class AStates<TActor, TSkin>
        {
            sealed protected class IdleState : AState
            {
                public IdleState(AStates<TActor, TSkin> parent) : base(parent) { }

                public override void Enter()
                {
                    Skin.Idle();
                    Actor.Interactable = Actor._canUseSkills = true;
                }

                public override void Exit()
                {
                    Actor.Interactable = Actor._canUseSkills = false;
                }

                public override void Select() => GameContainer.TriggerBus.TriggerActorSelect(Actor);
                public override void Unselect(ISelectable newSelectable) => GameContainer.TriggerBus.TriggerUnselect(Actor.Equals(newSelectable));
            }
        }
    }
}
