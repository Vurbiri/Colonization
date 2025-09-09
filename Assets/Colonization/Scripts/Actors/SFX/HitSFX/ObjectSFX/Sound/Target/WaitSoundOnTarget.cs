using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public class WaitSoundOnTarget : IHitSFX
    {
        private readonly AudioClip _clip;
        private readonly WaitRealtime _playTime;

        public WaitSoundOnTarget(AudioClip clip)
        {
            _clip = clip;
            _playTime = _clip.length;
        }

        public IEnumerator Hit(ActorSFX user, ActorSkin target)
        {
            target.Play(_clip);
            return _playTime.Restart();
        }
    }
}
