using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    sealed public class WarriorWarlockSFX : WarriorSFX
    {
        [Space]
        [SerializeField] private ParticleSystem _particle;

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
            this.SetChildren(ref _particle);
        }
#endif
    }
}
