using Vurbiri.FSM;

namespace Vurbiri.Colonization.FSMSelectable
{
    sealed public class StateMachineSelectable : StateMachine<ASelectableState>
    {
        public StateMachineSelectable() : base(new EmptyStateSelectable())
        {
        }

        public StateMachineSelectable(ASelectableState state) : base(state)
        {
        }

        public void Cancel() => _currentState.Cancel();

        public void Select() => _currentState.Select();

        public void Unselect(ISelectable newSelectable) => _currentState.Unselect(newSelectable);
    }
}
