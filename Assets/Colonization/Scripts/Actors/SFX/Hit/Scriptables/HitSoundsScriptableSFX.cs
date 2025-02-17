//Assets\Colonization\Scripts\Actors\SFX\Hit\Scriptables\HitSoundsScriptableSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    [CreateAssetMenu(fileName = "ASFX_", menuName = "Vurbiri/Colonization/ActorSFX/HitSounds", order = 51)]
	public class HitSoundsScriptableSFX : AHitScriptableSFX
    {
        [SerializeField] private AudioClip _clip;

        public override IHitSFX Create(IActorSFX parent) => new HitReactSoundSFX(_clip, parent.AudioSource);
    }
}
