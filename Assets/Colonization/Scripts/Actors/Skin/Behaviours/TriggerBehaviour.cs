//Assets\Colonization\Scripts\Actors\Skin\Behaviours\TriggerBehaviour.cs
using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class TriggerBehaviour : StateMachineBehaviour
	{
        public event Action EventExitTrigger;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => EventExitTrigger?.Invoke();
    }
}
