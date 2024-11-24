//Assets\Colonization\Scripts\Actors\Skin\States\BoolSwitchState.cs
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        private class BoolSwitchState : ASkinState
        {
            protected readonly int _idParam;

            public BoolSwitchState(string stateName, ActorSkin parent, int id = 0) : base(parent, id)
            {
                _idParam = Animator.StringToHash(stateName);
            }

            public override void Enter()
            {
                _animator.SetBool(_idParam, true);
            }

            public override void Exit()
            {
                _animator.SetBool(_idParam, false);
            }
        }
    }
}
