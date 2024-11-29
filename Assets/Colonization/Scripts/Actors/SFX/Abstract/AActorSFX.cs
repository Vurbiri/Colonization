//Assets\Colonization\Scripts\Actors\SFX\Abstract\AActorSFX.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public abstract class AActorSFX : MonoBehaviour
	{
        [SerializeField] protected float _heightDeath = -3.5f;
        [SerializeField] protected float _speedDeath = 1.25f;
        
        protected Transform _thisTransform;

        protected virtual void Awake()
		{
            _thisTransform = transform;
        }

        public virtual void Skill(int id, Transform target) { }
        public virtual void Hint(int id) { }

        public IEnumerator Death_Coroutine()
        {
            Vector3 position = _thisTransform.localPosition;
            while(position.y > _heightDeath)
            {
                yield return null;
                position.y -= _speedDeath * Time.deltaTime;
                _thisTransform.localPosition = position;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
			
        }
#endif
	}
}
