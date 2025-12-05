using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
	public abstract class AEnterBehaviour : StateMachineBehaviour
    {
        public event Action EventEnter;

        public AEnterBehaviour() : base() => EventEnter = Dummy.Action;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => EventEnter.Invoke();
    }
}
