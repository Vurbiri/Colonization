using System;

namespace Vurbiri.Colonization
{
    public interface ISelectable : IEquatable<ISelectable>
    {
        public void Select();
        public void Unselect(ISelectable newSelectable);
    }
}
