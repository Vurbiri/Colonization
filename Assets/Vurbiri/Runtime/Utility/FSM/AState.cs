using System.Runtime.CompilerServices;

namespace Vurbiri.FSM
{
    public abstract class AState : IState
    {
        protected readonly StateMachine _fsm;

        public AState(StateMachine fsm)
        {
            _fsm = fsm;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void GetOutOfThisState() => _fsm.GetOutState(this);

    }
}
