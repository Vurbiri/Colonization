//Assets\Colonization\Scripts\Actors\Skin\States\TriggerSwitchState.cs
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        public class TriggerSwitchState : AAnimatorState
        {
            protected readonly int _idParam;

            public TriggerSwitchState(string stateName, ActorSkin parent, int id = 0) : base(parent, id)
            {
                _idParam = Animator.StringToHash(stateName);
            }

            public override void Enter()
            {
                _animator.SetTrigger(_idParam);
            }

            public override void Exit()
            {
                _animator.ResetTrigger(_idParam);
            }
        }
    }
}
