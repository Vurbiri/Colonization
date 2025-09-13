using System.Collections;
using UnityEngine;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    [RequireComponent(typeof(AudioSource)), DisallowMultipleComponent]
    public abstract class ActorSFX : MonoBehaviour
    {
        protected ReadOnlyArray<string> _hitSFX;
        protected AudioSource _audioSource;

        public virtual Transform TargetTransform => transform;

        [Impl(256)] protected void InitInternal(ReadOnlyArray<string> hitSFX)
		{
            _audioSource = GetComponent<AudioSource>();
            _hitSFX = hitSFX;
        }

        [Impl(256)] public void Play(AudioClip clip) => _audioSource.PlayOneShot(clip);

        [Impl(256)] public IEnumerator Hit(int idSkill, ActorSkin target) => GameContainer.SFX.Run(_hitSFX[idSkill], this, target);

        public virtual void Death() { }

        public IEnumerator Death_Cn(float height)
        {
            var thisTransform = transform;
            Vector3 position = thisTransform.localPosition;
            while (position.y > height)
            {
                position.y -= 3f * Time.unscaledDeltaTime;
                thisTransform.localPosition = position;
                yield return null;
            }
        }
    }
}
