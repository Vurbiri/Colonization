//Assets\Vurbiri\Runtime\Utility\FSM\Base\IState.cs
using System;

namespace Vurbiri.FSM
{
    public interface IState : IDisposable, IEquatable<IState>
    {
        public TypeIdKey Key { get;  }

        public void Enter();

        public void Update();

        public void Exit();
    }
}
