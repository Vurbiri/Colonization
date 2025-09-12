using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public class IncludeSFX : IHitSFX
    {
        private readonly AudioClip _userClip;
        private readonly string _nameTarget;
        private readonly WaitRealtime _delay;

        public IncludeSFX(AudioClip userClip, string nameTarget, float delayTime)
        {
            _userClip = userClip;
            _nameTarget = nameTarget;
            if (delayTime > 0f)
                _delay = delayTime;
        }

        public IEnumerator Hit(ActorSFX user, ActorSkin target)
        {
            HitInternal(user, target).Start();
            return null;
        }

        private IEnumerator HitInternal(ActorSFX user, ActorSkin target)
        {
            user.Play(_userClip);
            yield return _delay;
            GameContainer.HitSFX.Hit(_nameTarget, user, target);
        }
    }
}
