using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class SoundOnUser : IHitSFX
    {
        private readonly AudioClip _clip;

        public SoundOnUser(AudioClip clip)
        {
            _clip = clip;
        }

        public IEnumerator Hit(ActorSFX user, ActorSkin target)
        {
            user.Play(_clip);
            return null;
        }
    }
}
