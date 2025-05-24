using System;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public abstract class AExitBehaviour : StateMachineBehaviour
    {
        public event Action EventExit;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => EventExit?.Invoke();
    }
}
