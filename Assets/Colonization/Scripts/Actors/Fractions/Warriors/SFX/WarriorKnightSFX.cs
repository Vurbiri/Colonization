using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public class WarriorKnightSFX : WarriorSFX
    {
        [Space]
        [SerializeField] private Transform _leftHand;
        [SerializeField] private ParticleSystem _particle;

        public override Vector3 StartPosition => _leftHand.position;

        protected override IEnumerator Start()
        {
            yield return base.Start();

            _particle.Play();
        }

        public override void Death()
        {
            _particle.Stop();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetChildren(ref _leftHand, "LeftHand");
            this.SetChildren(ref _particle);
        }
#endif
    }
}
