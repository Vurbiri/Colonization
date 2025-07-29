using System;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    sealed public class CreatorRangeParticleSFX : AMonoCreatorSFX
    {
        public AudioClip clipRun, clipHit;
        public ParticleSystem particle;

        public override APooledSFX Create(Action<APooledSFX> deactivate) => new RangeParticleSFX(this, deactivate);

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetComponent(ref particle);
        }
#endif
    }
}
