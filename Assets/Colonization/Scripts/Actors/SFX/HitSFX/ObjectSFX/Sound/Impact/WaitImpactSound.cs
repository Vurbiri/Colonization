using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
	public class WaitImpactSound : ISFX
    {
        private readonly AudioClip _clip;
        private readonly WaitRealtime _playTime;

        public WaitImpactSound(AudioClip clip)
        {
            _clip = clip;
            _playTime = _clip.length;
        }

        public IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            target.Impact(_clip);
            return _playTime.Restart();
        }
    }
}
