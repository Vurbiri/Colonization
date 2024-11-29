//Assets\Colonization\Scripts\Actors\Skin\States\Abstract\ASkinState.cs
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        protected class ASkinState : AState
        {
            protected readonly ActorSkin _parent;
            protected readonly Animator _animator;
            protected readonly AActorSFX _sfx;
            protected readonly int _idParam;

            public ASkinState(string stateName, ActorSkin parent, int id = 0) : base(parent._stateMachine, id)
            {
                _parent = parent;
                _animator = parent._animator;
                _sfx = parent._sfx;
                _idParam = Animator.StringToHash(stateName);
            }
        }
    }
}
