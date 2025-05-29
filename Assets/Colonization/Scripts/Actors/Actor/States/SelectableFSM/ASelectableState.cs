using Vurbiri.FSM;

namespace Vurbiri.Colonization.FSMSelectable
{
    public class ASelectableState : IState
    {
        protected readonly StateMachineSelectable _fsm;

        public ASelectableState(StateMachineSelectable fsm)
        {
            _fsm = fsm;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }

        public virtual void Cancel() { }
        public virtual void Select() { }
        public virtual void Unselect(ISelectable newSelectable) { }

        public virtual void Dispose() { }
    }
}
