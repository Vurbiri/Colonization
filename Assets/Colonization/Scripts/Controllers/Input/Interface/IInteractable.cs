using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public interface IInteractable : ISelectable, IPositionable, ICancel
    {
        public RBool InteractableReactive { get; }
        public bool Interactable { get; }
    }
}
