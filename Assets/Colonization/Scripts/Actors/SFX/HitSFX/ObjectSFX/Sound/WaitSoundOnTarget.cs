using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public class WaitSoundOnTarget : IHitSFX, IEnumerator
    {
        private readonly AudioClip _clip;
        private float _waitUntilTime;

        public WaitSoundOnTarget(AudioClip clip)
        {
            _clip = clip;
        }

        public object Current => null;

        public IEnumerator Hit(ISFXUser user, ActorSkin target)
        {
            
            target.ActorSFX.Impact(_clip);
            _waitUntilTime = Time.realtimeSinceStartup + _clip.length;
            this.Start();
            
            return this;
        }

        public bool MoveNext() => _waitUntilTime > Time.realtimeSinceStartup;

        public void Reset() { }
    }
}
