//Assets\Colonization\Scripts\Actors\Actor\States\IdleStates.cs
namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        public abstract class AIdleState : AState
        {
            public AIdleState(Actor parent) : base(parent) { }

            public override void Enter()
            {
                _skin.Idle();
            }
        }

        private class AIIdleState : AIdleState
        {
            public AIIdleState(Actor parent) : base(parent) {}
        }

        private class PlayerIdleState : AIdleState
        {
            public PlayerIdleState(Actor parent) : base(parent) { }

            public override void Select() => _eventBus.TriggerActorSelect(_actor);
            public override void Unselect(ISelectable newSelectable) => _eventBus.TriggerUnselect();
        }
    }
}
