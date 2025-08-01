using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    [CreateAssetMenu(fileName = "ASFX_", menuName = "Vurbiri/Colonization/ActorSFX/WaitTargetSound", order = 51)]
    public class WaitTargetSoundFactory : ASFXFactory
    {
        [SerializeField] private AudioClip _clip;

        public override IHitSFX Create() => new WaitSoundOnTarget(_clip);
    }
}
