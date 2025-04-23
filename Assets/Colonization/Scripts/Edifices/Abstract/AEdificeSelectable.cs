//Assets\Colonization\Scripts\Edifices\Abstract\AEdificeSelectable.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(Collider))]
    public abstract class AEdificeSelectable : AEdifice
    {
        [SerializeField] protected Collider _thisCollider;

        public override bool RaycastTarget { get => _thisCollider.enabled; set => _thisCollider.enabled = value; }


#if UNITY_EDITOR
        protected override void OnValidate()
        {
           base.OnValidate();

            if (_thisCollider == null)
                _thisCollider = GetComponent<Collider>();
        }
#endif
    }
}
