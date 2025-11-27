namespace Vurbiri.Colonization
{
    public interface ISelectable : System.IEquatable<ISelectable>
    {
        public void Select(MouseButton button);
        public void Unselect(ISelectable newSelectable);
    }
}
