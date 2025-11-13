using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    sealed public class ParticleCreator : AParticleCreatorSFX
    {
        [Space]
        [SerializeField] private SFXType _type;
        [SerializeField] private bool _isWait;

        public override APooledSFX Create(Action<APooledSFX> deactivate) => _type switch
        {
            SFXType.Impact => _isWait ? new WaitImpactParticle(this, deactivate) : new ImpactParticle(this, deactivate),
            SFXType.Target => _isWait ? new WaitParticleOnTarget(this, deactivate) : new ParticleOnTarget(this, deactivate),
            SFXType.User   => _isWait ? new WaitParticleOnUser(this, deactivate) : new ParticleOnUser(this, deactivate),
            _ => null
        };

#if UNITY_EDITOR
        public override TargetForSFX_Ed Target_Ed => _type == SFXType.User ? TargetForSFX_Ed.User : TargetForSFX_Ed.Target;
#endif
    }
}
