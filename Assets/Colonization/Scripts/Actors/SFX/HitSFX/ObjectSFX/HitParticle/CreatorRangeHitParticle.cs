using System;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    sealed public class CreatorRangeHitParticle : AMonoCreatorSFX
    {
        public AudioClip clipRun, clipHit;
        public ParticleSystem particle;
        public float targetHeightRate;

        public override APooledSFX Create(Action<APooledSFX> deactivate) => new RangeHitParticle(this, deactivate);

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetComponent(ref particle);
        }
#endif
    }
}
