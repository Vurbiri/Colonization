//Assets\Vurbiri\Runtime\Utilities\FSM\Base\IState.cs
using System;

namespace Vurbiri.FSM
{
    public interface IState : IDisposable, IEquatable<IState>
    {
        public TypeIdKey Key { get;  }

        public void Enter();

        public void Exit();

        public void Update();
    }
}
