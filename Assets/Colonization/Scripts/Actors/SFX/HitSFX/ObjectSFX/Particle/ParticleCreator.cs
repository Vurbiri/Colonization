using System;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    sealed public class ParticleCreator : AMonoCreatorSFX
    {
        [SerializeField] private SFXType _type;
        [SerializeField] private bool _isWait;
        [Space]
        [SerializeField] private AudioClip _clip;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private float _targetHeightRate;

        public AudioClip Clip => _clip;
        public ParticleSystem Particle => _particle;
        public float HeightRate => _targetHeightRate;

        public override APooledSFX Create(Action<APooledSFX> deactivate) => _type switch
        {
            SFXType.Impact => _isWait ? new WaitImpactParticle(this, deactivate) : new ImpactParticle(this, deactivate),
            SFXType.Target => _isWait ? new WaitParticleOnTarget(this, deactivate) : new ParticleOnTarget(this, deactivate),
            SFXType.User   => _isWait ? new WaitParticleOnUser(this, deactivate) : new ParticleOnUser(this, deactivate),
            _ => null
        };

#if UNITY_EDITOR
        public override TargetForSFX_Ed Target_Ed => _type == SFXType.User ? TargetForSFX_Ed.User : TargetForSFX_Ed.Target;

        private void OnValidate()
        {
            this.SetComponent(ref _particle);
        }
#endif
    }
}
