using System;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public class CreatorInstantHitParticle : AMonoCreatorSFX
    {
        public AudioClip clip;
        public ParticleSystem particle;

        public override APooledSFX Create(Action<APooledSFX> deactivate) => new InstantHitParticle(this, deactivate);

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetComponent(ref particle);
        }
#endif
    }
}
