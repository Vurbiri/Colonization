using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization
{
	public abstract class AIncludeSFX
    {
        private readonly AudioClip _userClip;
        private readonly string _nameTarget;
        private readonly WaitRealtime _delay;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AIncludeSFX(AudioClip userClip, string nameTarget, float delayTime)
        {
            _userClip = userClip;
            _nameTarget = nameTarget;
            if (delayTime > 0f)
                _delay = delayTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected IEnumerator RunInternal(ActorSFX user, ActorSkin target)
        {
            user.Play(_userClip);
            yield return _delay;
            yield return GameContainer.SFX.Run(_nameTarget, user, target);
        }
    }
}
