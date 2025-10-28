using System;

namespace Vurbiri.Colonization
{
    public interface ISelectable : IEquatable<ISelectable>
    {
        public void Select(MouseButton button);
        public void Unselect(ISelectable newSelectable);
    }
}
