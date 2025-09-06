using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public abstract class ACreatorParticle : AMonoCreatorSFX
	{
        public AudioClip clip;
        public ParticleSystem particle;
        public float targetHeightRate;

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetComponent(ref particle);
        }
#endif
    }
}
