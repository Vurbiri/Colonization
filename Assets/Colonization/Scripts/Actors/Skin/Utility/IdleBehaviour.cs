using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class IdleBehaviour : StateMachineBehaviour
    {
        private int _idIdle;
        public int IdIdle {  set => _idIdle = value; }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => animator.SetInteger(_idIdle, 0);
    }
}
