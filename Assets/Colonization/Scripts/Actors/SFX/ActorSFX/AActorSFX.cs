using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class AActorSFX : MonoBehaviour, IUserSFX
    {
        [SerializeField] protected float _heightDeath = -3.5f;
        [SerializeField] protected float _durationDeath = 1f;
        [ReadOnly, SerializeField] protected SkillSFX[] _skillSFX;

        protected Transform _thisTransform;
        protected AudioSource _audioSource;

        public abstract Vector3 StartPosition { get; }
        public AudioSource AudioSource => _audioSource;

        protected virtual void Awake()
		{
            _thisTransform = transform;
            _audioSource = GetComponent<AudioSource>();
        }

        public virtual void Impact(AudioClip clip) => _audioSource.PlayOneShot(clip);

        public virtual void Block(bool isActive) { }

        public virtual IEnumerator Hit(int idSkill, int idHit, ActorSkin target) => GameContainer.HitSFX.Hit(_skillSFX[idSkill][idHit], this, target);

        public virtual void Death() { }

        public virtual IEnumerator Death_Cn()
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

        #region Nested: SkillSFX
        //***********************************
        [Serializable]
        protected class SkillSFX
        {
            [SerializeField] private string[] _hitSFX;

            public string this[int index] => _hitSFX[index];

#if UNITY_EDITOR
            public void SetSize_Ed(int count)
            {
                if (_hitSFX == null || _hitSFX.Length != count)
                    _hitSFX = new string[count];
            }
            public void SetValue_Ed(int index, string value) => _hitSFX[index] = value;
#endif

        }
        #endregion

#if UNITY_EDITOR

        public void SetCountSkillsSFX_Ed(int count)
        {
            if (_skillSFX == null || _skillSFX.Length != count)
            {
                _skillSFX = new SkillSFX[count];
                for (int i = 0; i < count; i++)
                    _skillSFX[i] = new();
            }
        }
        public void SetCountHitsSFX_Ed(int idSkill, int count) => _skillSFX[idSkill].SetSize_Ed(count);
        public void SetSkillSFX_Ed(int idSkill, int idHit, string sfx) => _skillSFX[idSkill].SetValue_Ed(idHit, sfx);
#endif
    }
}
