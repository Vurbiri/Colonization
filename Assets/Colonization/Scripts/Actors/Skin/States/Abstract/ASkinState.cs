//Assets\Colonization\Scripts\Actors\Skin\States\Abstract\ASkinState.cs
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        private class ASkinState : AState
        {
            protected readonly ActorSkin _parent;
            protected readonly Animator _animator;

            public ASkinState(ActorSkin parent, int id = 0) : base(parent._stateMachine, id)
            {
                _parent = parent;
                _animator = parent._animator;
            }
        }
    }
}
