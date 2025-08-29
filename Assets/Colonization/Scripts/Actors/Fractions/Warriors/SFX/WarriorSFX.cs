using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class WarriorSFX : ActorSFX
    {
        [Space]
        [SerializeField] protected float _startScale = 0.01f;
        [SerializeField] protected float _durationScaling = 0.8f;

        public override Vector3 StartPosition { get; } = Vector3.zero;

        protected virtual IEnumerator Start()
        {
            Vector3 current = _startScale * Vector3.one;
            float speed = (1f - _startScale) / _durationScaling;
            while(_startScale <= 1f )
            {
                _thisTransform.localScale = current;
                yield return null;
                _startScale += speed * Time.deltaTime;
                current.x = current.y = current.z = _startScale;
            }
            _thisTransform.localScale = Vector3.one;
        }

        public virtual void Block(bool isActive) { }
    }
}
