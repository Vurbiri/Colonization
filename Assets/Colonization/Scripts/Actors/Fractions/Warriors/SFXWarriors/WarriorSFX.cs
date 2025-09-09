using System.Collections;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Actors
{
    public class WarriorSFX : ActorSFX
    {
        [Space]
        [SerializeField] protected float _startScale = 0.01f;
        [SerializeField] protected float _durationScaling = 0.8f;

        protected virtual IEnumerator Start()
        {
            var thisTransform = transform;
            Vector3 current = _startScale * Vector3.one;
            float speed = (1f - _startScale) / _durationScaling;
            while(_startScale <= 1f )
            {
                thisTransform.localScale = current;
                yield return null;
                _startScale += speed * Time.deltaTime;
                current.x = current.y = current.z = _startScale;
            }
            thisTransform.localScale = Vector3.one;
        }

        public void Init(ReadOnlyArray<string> hitSFX) => InitInternal(hitSFX);

        public virtual void Block(bool isActive) { }
    }
}
