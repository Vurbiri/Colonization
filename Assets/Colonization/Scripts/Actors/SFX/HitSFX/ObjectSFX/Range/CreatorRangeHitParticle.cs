using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    sealed public class CreatorRangeHitParticle : AMonoCreatorSFX
    {
        public AudioClip clipRun, clipHit;
        [Space]
        public ParticleSystem particle;
        public float particleLifetimeRate = 1f;
        [Space]
        public float targetHeightRate;

        public override APooledSFX Create(Action<APooledSFX> deactivate) => new RangeHitParticle(this, deactivate);

#if UNITY_EDITOR
        public override TargetForSFX_Ed Target_Ed => TargetForSFX_Ed.Combo;


        private void OnValidate()
        {
            this.SetComponent(ref particle);
        }
#endif
    }
}
