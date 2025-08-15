using System;
using System.Runtime.CompilerServices;

namespace Vurbiri.FSM
{
    public abstract class AState : IState, IEquatable<AState>
    {
        private readonly StateMachine _fsm;

        public AState(StateMachine fsm)
        {
            _fsm = fsm;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void GetOutOfThisState() => _fsm.GetOutState(this);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void GetOutToPrevState() => _fsm.GetOutToPrevState(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(AState other) => ReferenceEquals(this, other);
    }
}
