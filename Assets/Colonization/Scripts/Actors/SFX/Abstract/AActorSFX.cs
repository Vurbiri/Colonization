//Assets\Colonization\Scripts\Actors\SFX\Abstract\AActorSFX.cs
using System;
using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract class AActorSFX : MonoBehaviour
	{
        [SerializeField] protected float _heightDeath = -3.5f;
        [SerializeField] protected float _durationDeath = 1f;
        [HideInInspector, SerializeField] protected ASkillMonoSFX[] _skillsSFX;
        [HideInInspector, SerializeField] protected float[] _durationsSFX;

        protected Transform _thisTransform;
        protected ISkillSFX[] _skills;

        protected virtual void Awake()
		{
            _thisTransform = transform;

            int count = _skillsSFX.Length;
            _skills = new ISkillSFX[count];

            for (int i = 0; i < count; i++)
            {
                if (_skillsSFX[i] == null)
                    _skills[i] = new SkillEmptySFX();
                else
                    _skills[i] = _skillsSFX[i].Create(_thisTransform, _durationsSFX[i]);
            }

            _skillsSFX = null;
            _durationsSFX = null;
        }

        public virtual void Skill(int id, Transform target) 
        {
           _skills[id].Run(target);
        }
        public virtual void Hit(int id) { }

        public IEnumerator Death_Coroutine()
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

#if UNITY_EDITOR

        public void SetSkillSFX(ASkillMonoSFX sfx, float duration, int id)
        {
            if (_skillsSFX == null || _skillsSFX.Length == 0 || _durationsSFX == null || _durationsSFX.Length == 0)
                return;

            _skillsSFX[id] = sfx;
            _durationsSFX[id] = duration;
        }

        public void SetCountSkillsSFX(int count)
        {
            _skillsSFX ??= new ASkillMonoSFX[count];
            if (_skillsSFX.Length != count)
                Array.Resize(ref _skillsSFX, count);

            _durationsSFX ??= new float[count];
            if (_durationsSFX.Length != count)
                Array.Resize(ref _durationsSFX, count);
        }
#endif
	}
}
