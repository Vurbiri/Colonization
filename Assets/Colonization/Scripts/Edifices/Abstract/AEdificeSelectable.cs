//Assets\Colonization\Scripts\Edifices\Abstract\AEdificeSelectable.cs
using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(Collider))]
    public abstract class AEdificeSelectable : AEdifice, ISelectable
    {
        [SerializeField] protected Collider _collider;

        private Action eventSelect;
        private Action<ISelectable> eventUnselect;

        public override bool ColliderEnable { get => _collider.enabled; set => _collider.enabled = value; }

        public override void Subscribe(Action onSelect, Action<ISelectable> onUnselect)
        {
            eventSelect = onSelect;
            eventUnselect = onUnselect;
        }

        public void Select() => eventSelect?.Invoke();
        public void Unselect(ISelectable newSelectable) => eventUnselect?.Invoke(newSelectable);

#if UNITY_EDITOR
        protected override void OnValidate()
        {
           base.OnValidate();

            if (_collider == null)
                _collider = GetComponent<Collider>();
        }
#endif
    }
}
