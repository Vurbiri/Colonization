using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public interface IInteractable : ISelectable, IPositionable, ICancel
    {
        public ReactiveValue<bool> InteractableReactive { get; }
        public bool Interactable { get; }
    }
}
