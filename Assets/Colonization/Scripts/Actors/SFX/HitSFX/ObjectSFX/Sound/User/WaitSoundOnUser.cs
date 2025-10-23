using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
	public class WaitSoundOnUser : ISFX
    {
        private readonly AudioClip _clip;
        private readonly WaitRealtime _playTime;

        public WaitSoundOnUser(AudioClip clip)
        {
            _clip = clip;
            _playTime = _clip.length;
        }

        public IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            user.Play(_clip);
            return _playTime.Restart();
        }
    }
}
