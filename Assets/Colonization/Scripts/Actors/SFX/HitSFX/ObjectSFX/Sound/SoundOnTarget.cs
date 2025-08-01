using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public class SoundOnTarget : IHitSFX
    {
        private readonly AudioClip _clip;

        public SoundOnTarget(AudioClip clip)
        {
            _clip = clip;
        }

        public IEnumerator Hit(ISFXUser user, ActorSkin target)
        {
            target.ActorSFX.AudioSource.PlayOneShot(_clip);

            return null;
        }
    }
}
