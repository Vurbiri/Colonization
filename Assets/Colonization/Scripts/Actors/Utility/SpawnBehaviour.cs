//Assets\Colonization\Scripts\Actors\Utility\SpawnBehaviour.cs
using System;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class SpawnBehaviour : StateMachineBehaviour
    {
        public event Action EventExitSpawn;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => EventExitSpawn?.Invoke();
    }
}
