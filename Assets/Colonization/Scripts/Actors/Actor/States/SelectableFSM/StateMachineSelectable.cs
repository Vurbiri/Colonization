using Vurbiri.FSM;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

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

        [Impl(256)] public void Cancel() => _currentState.Cancel();

        [Impl(256)] public void Select() => _currentState.Select();

        [Impl(256)] public void Unselect(ISelectable newSelectable) => _currentState.Unselect(newSelectable);
    }
}
