using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class SoundOnUser : ISFX
    {
        private readonly AudioClip _clip;

        public SoundOnUser(AudioClip clip)
        {
            _clip = clip;
        }

        public IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            user.Play(_clip);
            yield break;
        }
    }
}
