//Assets\Colonization\Scripts\Actors\SFX\Abstract\AWarriorSFX.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public abstract class AWarriorSFX : AActorSFX
    {
        [Space]
        [SerializeField] protected float _scale = 0.01f;
        [SerializeField] protected float _durationScaling = 0.9f;

        protected virtual IEnumerator Start()
        {
            Vector3 current = _scale * Vector3.one;
            float speed = (1f - _scale) / _durationScaling;
            while(_scale <= 1f )
            {
                _thisTransform.localScale = current;
                yield return null;
                _scale += speed * Time.deltaTime;
                current.x = current.y = current.z = _scale;
            }
            _thisTransform.localScale = Vector3.one;
        }
    }
}
