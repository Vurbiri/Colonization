namespace Vurbiri.Colonization
{
    public interface ISelectable
    {
        public void Select();
        public void Unselect(ISelectable newSelectable);
    }
}
