using System;

namespace Vurbiri.FSM
{
    public interface IState : IDisposable
    {
        public void Enter();

        public void Exit();
    }
}
