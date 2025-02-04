//Assets\Colonization\Scripts\Actors\SFX\Hit\Scriptables\HitSoundsScriptableSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    [CreateAssetMenu(fileName = "ScriptableHitSoundSFX", menuName = "Vurbiri/Colonization/ActorSFX/HitSoundsScriptableSFX", order = 51)]
	public class HitSoundsScriptableSFX : AHitScriptableSFX
    {
        [SerializeField] private AudioClip _clip;

        public override IHitSFX Create(IActorSFX parent) => new HitSoundSFX(_clip, parent.AudioSource);
    }
}
