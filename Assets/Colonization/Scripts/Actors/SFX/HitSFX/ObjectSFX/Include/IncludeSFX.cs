using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    sealed public class IncludeSFX : AIncludeSFX, ISFX
    {
        public IncludeSFX(AudioClip userClip, string nameTarget, float delayTime) : base(userClip, nameTarget, delayTime) { }

        public IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            RunInternal(user, target).Start();
            return null;
        }

    }
}
