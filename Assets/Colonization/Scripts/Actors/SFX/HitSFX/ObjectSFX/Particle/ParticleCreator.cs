using System;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class ParticleCreator : AMonoCreatorSFX
    {
        [SerializeField] private bool _isImpact;
        [SerializeField] private bool _isWait;
        [Space]
        [SerializeField] private AudioClip _clip;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private float _targetHeightRate;

        public AudioClip Clip => _clip;
        public ParticleSystem Particle => _particle;
        public float HeightRate => _targetHeightRate;

        public override APooledSFX Create(Action<APooledSFX> deactivate)
        {
            if (_isImpact)
                return _isWait ? new WaitHitParticle(this, deactivate) : new InstantHitParticle(this, deactivate);
            else
                return _isWait ? new WaitParticle(this, deactivate) : new InstantParticle(this, deactivate);
        }

#if UNITY_EDITOR
        public override TargetForSFX_Ed Target_Ed => TargetForSFX_Ed.Target;

        private void OnValidate()
        {
            this.SetComponent(ref _particle);
        }
#endif
    }
}
