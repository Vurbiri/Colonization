using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class ImpactSound : ISFX
    {
        private readonly AudioClip _clip;

        public ImpactSound(AudioClip clip)
        {
            _clip = clip;
        }

        public IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            target.Impact(_clip);
            yield break;
        }
    }
}
