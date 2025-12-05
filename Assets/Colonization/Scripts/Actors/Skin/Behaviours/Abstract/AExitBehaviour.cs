using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class AExitBehaviour : StateMachineBehaviour
    {
        public event Action EventExit;

        public AExitBehaviour() : base() => EventExit = Dummy.Action;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => EventExit.Invoke();
    }
}
