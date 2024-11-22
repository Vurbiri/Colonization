//Assets\Colonization\Scripts\Utility\SelectableFSM\StateMachineSelectable.cs
using Vurbiri.FSM;

namespace Vurbiri.Colonization.FSMSelectable
{
    public class StateMachineSelectable : StateMachine<ASelectableState>, ISelectable
    {
        public StateMachineSelectable() : base(new EmptyStateSelectable())
        {
        }

        public void Select() => _currentState.Select();

        public void Unselect(ISelectable newSelectable) => _currentState.Unselect(newSelectable);
    }
}
