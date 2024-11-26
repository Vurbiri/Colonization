//Assets\Colonization\Scripts\Actors\SFX\Abstract\ASFX.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public abstract class ASFX : MonoBehaviour
	{
        [SerializeField] private float _heightDeath = -3.5f;
        [SerializeField] private float _speedDeath = 1.25f;
        
        protected Transform _thisTransform;

        private void Start()
		{
            _thisTransform = transform;
        }

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
