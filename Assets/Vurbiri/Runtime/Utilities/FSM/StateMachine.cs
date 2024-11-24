//Assets\Vurbiri\Runtime\Utilities\FSM\StateMachine.cs
namespace Vurbiri.FSM
{
    public class StateMachine : StateMachine<AState>
    {
        public StateMachine() : base(new EmptyState()) { }
        public StateMachine(AState state) : base(state) { }
    }
}
