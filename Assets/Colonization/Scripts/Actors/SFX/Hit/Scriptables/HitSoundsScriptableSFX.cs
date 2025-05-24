using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    [CreateAssetMenu(fileName = "ASFX_", menuName = "Vurbiri/Colonization/ActorSFX/HitSounds", order = 51)]
	public class HitSoundsScriptableSFX : AHitScriptableSFX
    {
        [SerializeField] private AudioClip _clip;

        public override IHitSFX Create(IDataSFX parent) => new HitReactSoundSFX(_clip);
    }
}
