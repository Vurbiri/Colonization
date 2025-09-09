using System.Collections;
using UnityEngine;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    [RequireComponent(typeof(AudioSource)), DisallowMultipleComponent]
    public abstract class ActorSFX : MonoBehaviour
    {
        [SerializeField] protected float _heightDeath; // = -3.5f;
        [SerializeField] protected float _durationDeath; // = 1f;

        protected ReadOnlyArray<string> _hitSFX;
        protected AudioSource _audioSource;

        public virtual Vector3 Position => transform.position;

        [Impl(256)] protected void InitInternal(ReadOnlyArray<string> hitSFX)
		{
            _audioSource = GetComponent<AudioSource>();
            _hitSFX = hitSFX;
        }

        [Impl(256)] public void Play(AudioClip clip) => _audioSource.PlayOneShot(clip);

        [Impl(256)] public IEnumerator Hit(int idSkill, ActorSkin target) => GameContainer.HitSFX.Hit(_hitSFX[idSkill], this, target);

        public virtual void Death() { }

        public IEnumerator Death_Cn()
        {
            var thisTransform = transform;
            Vector3 position = thisTransform.localPosition;
            float speed = _heightDeath / _durationDeath;
            while (position.y > _heightDeath)
            {
                yield return null;
                position.y += speed * Time.deltaTime;
                thisTransform.localPosition = position;
            }
        }
    }
}
