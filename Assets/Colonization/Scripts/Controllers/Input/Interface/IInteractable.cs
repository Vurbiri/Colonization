using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public interface IInteractable : ISelectable, IPositionable, ICancel
    {
        public Reactive<bool> InteractableReactive { get; }
        public bool Interactable { get; }
    }
}
