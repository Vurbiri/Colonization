using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public class SoundOnTarget : ISFX
    {
        private readonly AudioClip _clip;

        public SoundOnTarget(AudioClip clip)
        {
            _clip = clip;
        }

        public IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            target.Play(_clip);
            yield break;
        }
    }
}
