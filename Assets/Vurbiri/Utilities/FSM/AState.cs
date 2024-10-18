using System;

namespace Vurbiri.FSM
{
    public abstract class AState : IDisposable
    {
        protected StateMachine _fsm;
        
        public AState(StateMachine fsm) => _fsm = fsm;

        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual void Update() { }


        public virtual void Dispose() { }
    }
}
