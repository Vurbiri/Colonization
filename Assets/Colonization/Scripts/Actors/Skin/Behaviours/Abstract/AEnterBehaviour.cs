using System;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public abstract class AEnterBehaviour : StateMachineBehaviour
    {
        public event Action EventEnter;

        public AEnterBehaviour() : base() => EventEnter = Empty;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => EventEnter.Invoke();

        private void Empty() { }
    }
}
