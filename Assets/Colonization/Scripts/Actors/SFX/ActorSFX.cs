using System.Collections;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Actors
{
    [RequireComponent(typeof(AudioSource)), DisallowMultipleComponent]
    public abstract class ActorSFX : MonoBehaviour, ISFXUser
    {
        [SerializeField] protected float _heightDeath = -3.5f;
        [SerializeField] protected float _durationDeath = 1f;

        protected ReadOnlyArray<string> _hitSFX;
        protected Transform _thisTransform;
        protected AudioSource _audioSource;

        public abstract Vector3 StartPosition { get; }
        public AudioSource AudioSource => _audioSource;

        public void Init(ReadOnlyArray<string> hitSFX)
		{
            _thisTransform = transform;
            _audioSource = GetComponent<AudioSource>();
            _hitSFX = hitSFX;
        }

        public void Impact(AudioClip clip) => _audioSource.PlayOneShot(clip);

        public IEnumerator Hit(int idSkill, ActorSkin target) => GameContainer.HitSFX.Hit(_hitSFX[idSkill], this, target);

        public virtual void Death() { }

        public IEnumerator Death_Cn()
        {
            Vector3 position = _thisTransform.localPosition;
            float speed = _heightDeath / _durationDeath;
            while (position.y > _heightDeath)
            {
                yield return null;
                position.y += speed * Time.deltaTime;
                _thisTransform.localPosition = position;
            }
        }
    }
}
