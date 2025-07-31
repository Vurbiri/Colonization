using System;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class CreatorInstantParticle : AMonoCreatorSFX
    {
        public AudioClip clip;
        public ParticleSystem particle;

        public override APooledSFX Create(Action<APooledSFX> deactivate) => new InstantParticle(this, deactivate);

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetComponent(ref particle);
        }
#endif
    }
}
