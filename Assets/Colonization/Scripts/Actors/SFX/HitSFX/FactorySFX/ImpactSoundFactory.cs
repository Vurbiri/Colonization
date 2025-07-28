using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    [CreateAssetMenu(fileName = "ASFX_", menuName = "Vurbiri/Colonization/ActorSFX/ImpactSound", order = 51)]
	public class ImpactSoundFactory : ASFXFactory
    {
        [SerializeField] private AudioClip _clip;

        public override IHitSFX Create() => new ImpactSound(_clip);
    }
}
