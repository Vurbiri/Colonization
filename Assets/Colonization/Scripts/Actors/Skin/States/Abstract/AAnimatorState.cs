//Assets\Colonization\Scripts\Actors\Skin\States\Abstract\AAnimatorState.cs
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin : MonoBehaviour
    {
        public class AAnimatorState : AState
        {
            protected readonly ActorSkin _parent;
            protected readonly Animator _animator;

            public AAnimatorState(ActorSkin parent, int id = 0) : base(parent._stateMachine, id)
            {
                _parent = parent;
                _animator = parent._animator;
            }
        }
    }
}
