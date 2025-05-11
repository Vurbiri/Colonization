//Assets\Colonization\Scripts\Controllers\Input\Interface\ISelectableInteractable.cs
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public interface IInteractable : ISelectable, IPositionable, ICancel
    {
        public RBool InteractableReactive { get; }
        public bool Interactable { get; }
    }
}
