namespace Vurbiri.FSM
{
    public class StateMachine : StateMachine<AState>
    {
        public StateMachine() : base(new EmptyState()) { }
    }
}
