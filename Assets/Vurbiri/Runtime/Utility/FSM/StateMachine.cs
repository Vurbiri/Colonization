//Assets\Vurbiri\Runtime\Utility\FSM\StateMachine.cs
namespace Vurbiri.FSM
{
    public class StateMachine : StateMachine<AState>
    {
        public StateMachine() : base(new EmptyState()) { }
        public StateMachine(AState state) : base(state) { }
    }
}
