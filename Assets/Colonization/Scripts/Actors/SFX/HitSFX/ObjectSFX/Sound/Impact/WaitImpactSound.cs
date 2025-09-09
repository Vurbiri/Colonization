using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public class WaitImpactSound : IHitSFX
    {
        private readonly AudioClip _clip;
        private readonly WaitRealtime _playTime;

        public WaitImpactSound(AudioClip clip)
        {
            _clip = clip;
            _playTime = _clip.length;
        }

        public IEnumerator Hit(ActorSFX user, ActorSkin target)
        {
            target.Impact(_clip);
            return _playTime.Restart();
        }
    }
}
